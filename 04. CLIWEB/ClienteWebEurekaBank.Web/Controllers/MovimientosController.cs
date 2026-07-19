using ClienteWebEurekaBank.Web.Helpers;
using ClienteWebEurekaBank.Web.Interfaces;
using ClienteWebEurekaBank.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWebEurekaBank.Web.Controllers;

[ExigirSesion]
public sealed class MovimientosController : Controller
{
    private readonly IApiEurekaServicio _apiEurekaServicio;

    public MovimientosController(IApiEurekaServicio apiEurekaServicio)
    {
        _apiEurekaServicio = apiEurekaServicio;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View("/Vistas/Movimientos/Index.cshtml", new MovimientoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Consultar(MovimientoViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Movimientos/Index.cshtml", modelo);
        }

        var (respuesta, movimientos) = await _apiEurekaServicio.ObtenerMovimientosAsync(modelo.Cuenta, cancellationToken);
        if (!respuesta.EsExitoso)
        {
            TempData["MensajeError"] = respuesta.Mensaje;
            modelo.Movimientos = new();
            return View("/Vistas/Movimientos/Index.cshtml", modelo);
        }

        modelo.Movimientos = movimientos;
        TempData["MensajeExito"] = respuesta.Mensaje;
        return View("/Vistas/Movimientos/Index.cshtml", modelo);
    }
}
