using ClienteWebEurekaBank.Web.Helpers;
using ClienteWebEurekaBank.Web.Interfaces;
using ClienteWebEurekaBank.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWebEurekaBank.Web.Controllers;

public sealed class AccesoController : Controller
{
    private readonly IApiEurekaServicio _apiEurekaServicio;

    public AccesoController(IApiEurekaServicio apiEurekaServicio)
    {
        _apiEurekaServicio = apiEurekaServicio;
    }

    [HttpGet]
    public IActionResult IniciarSesion()
    {
        if (!string.IsNullOrWhiteSpace(HttpContext.Session.GetString(ConstantesSesion.UsuarioAutenticado)))
        {
            return RedirectToAction("Index", "Panel");
        }

        return View("/Vistas/Acceso/IniciarSesion.cshtml", new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IniciarSesion(LoginViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Acceso/IniciarSesion.cshtml", modelo);
        }

        var respuesta = await _apiEurekaServicio.ValidarUsuarioAsync(modelo.Usuario, modelo.Password, cancellationToken);
        if (!respuesta.EsExitoso)
        {
            ModelState.AddModelError(string.Empty, respuesta.Mensaje);
            return View("/Vistas/Acceso/IniciarSesion.cshtml", modelo);
        }

        HttpContext.Session.SetString(ConstantesSesion.UsuarioAutenticado, modelo.Usuario);
        TempData["MensajeExito"] = respuesta.Mensaje;
        return RedirectToAction("Index", "Panel");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CerrarSesion()
    {
        HttpContext.Session.Clear();
        TempData["MensajeExito"] = "Sesión cerrada correctamente.";
        return RedirectToAction("IniciarSesion", "Acceso");
    }
}
