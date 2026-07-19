using System.Net;
using ClienteWebEurekaBank.Web.Configuracion;
using ClienteWebEurekaBank.Web.Interfaces;
using ClienteWebEurekaBank.Web.Servicios;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Polly;

var constructorAplicacion = WebApplication.CreateBuilder(args);

constructorAplicacion.Services.AddControllersWithViews();

constructorAplicacion.Services.AddDistributedMemoryCache();

constructorAplicacion.Services.Configure<ApiEurekaOpciones>(
    constructorAplicacion.Configuration.GetSection(ApiEurekaOpciones.Seccion));

// Configuración del servidor (URL/IP del API) modificable en caliente desde la interfaz.
// Se inicializa con el valor de appsettings.json y se persiste en un archivo JSON.
constructorAplicacion.Services.AddSingleton<IServidorApiConfig>(proveedor =>
{
    var opcionesApi = proveedor.GetRequiredService<IOptions<ApiEurekaOpciones>>().Value;
    var rutaArchivo = Path.Combine(constructorAplicacion.Environment.ContentRootPath, "servidor.runtime.json");
    return new ServidorApiConfig(opcionesApi.UrlBase, rutaArchivo);
});

constructorAplicacion.Services.AddSession(opciones =>
{
    opciones.IdleTimeout = TimeSpan.FromMinutes(30);
    opciones.Cookie.HttpOnly = true;
    opciones.Cookie.IsEssential = true;
});

constructorAplicacion.Services.Configure<RazorViewEngineOptions>(opciones =>
{
    // Mantiene /Views y agrega /Vistas como ubicación adicional.
    opciones.ViewLocationFormats.Add("/Vistas/{1}/{0}.cshtml");
    opciones.ViewLocationFormats.Add("/Vistas/Shared/{0}.cshtml");
});

constructorAplicacion.Services.AddHttpClient<IApiEurekaServicio, ApiEurekaServicio>((proveedor, clienteHttp) =>
{
    // Se lee la URL vigente del servidor (puede haberse cambiado desde la interfaz).
    var servidorApiConfig = proveedor.GetRequiredService<IServidorApiConfig>();
    var urlBaseActual = servidorApiConfig.UrlBase;
    if (string.IsNullOrWhiteSpace(urlBaseActual))
    {
        throw new InvalidOperationException($"Falta configurar '{ApiEurekaOpciones.Seccion}:UrlBase' en appsettings.json");
    }

    var urlBase = urlBaseActual.TrimEnd('/') + "/";
    clienteHttp.BaseAddress = new Uri(urlBase, UriKind.Absolute);
    clienteHttp.Timeout = TimeSpan.FromSeconds(20);
})
.AddPolicyHandler(Policy<HttpResponseMessage>
    .HandleResult(respuesta => respuesta.StatusCode == HttpStatusCode.InternalServerError)
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: intento => TimeSpan.FromMilliseconds(250 * intento)));

var aplicacion = constructorAplicacion.Build();

if (!aplicacion.Environment.IsDevelopment())
{
    aplicacion.UseExceptionHandler("/Panel/Error");
    aplicacion.UseHsts();
}

aplicacion.UseHttpsRedirection();
aplicacion.UseStaticFiles();

aplicacion.UseRouting();

aplicacion.UseSession();
aplicacion.UseAuthorization();

aplicacion.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=IniciarSesion}/{id?}");

aplicacion.Run();
