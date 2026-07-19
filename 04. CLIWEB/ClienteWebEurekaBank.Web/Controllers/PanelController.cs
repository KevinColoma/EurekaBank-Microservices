using System.Diagnostics;
using ClienteWebEurekaBank.Web.Helpers;
using ClienteWebEurekaBank.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWebEurekaBank.Web.Controllers;

[ExigirSesion]
public sealed class PanelController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Usuario = HttpContext.Session.GetString(ConstantesSesion.UsuarioAutenticado);
        return View("/Vistas/Panel/Index.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View("/Vistas/Panel/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
