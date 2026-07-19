using ClienteWebEurekaBank.Web.Configuracion;
using ClienteWebEurekaBank.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWebEurekaBank.Web.Controllers;

public sealed class ConfiguracionController : Controller
{
    private readonly IServidorApiConfig _servidorApiConfig;

    public ConfiguracionController(IServidorApiConfig servidorApiConfig)
    {
        _servidorApiConfig = servidorApiConfig;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var modelo = new ConfiguracionServidorViewModel { UrlBase = _servidorApiConfig.UrlBase };
        return View("/Vistas/Configuracion/Index.cshtml", modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ConfiguracionServidorViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Configuracion/Index.cshtml", modelo);
        }

        var url = modelo.UrlBase?.Trim() ?? string.Empty;
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            ModelState.AddModelError(nameof(modelo.UrlBase),
                "Ingrese una URL válida, por ejemplo: http://192.168.1.10:5069/Eureka");
            return View("/Vistas/Configuracion/Index.cshtml", modelo);
        }

        _servidorApiConfig.Actualizar(url);
        TempData["MensajeExito"] = "Servidor actualizado correctamente.";
        return RedirectToAction("Index");
    }
}
