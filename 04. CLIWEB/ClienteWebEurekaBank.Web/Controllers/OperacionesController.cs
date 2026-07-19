using ClienteWebEurekaBank.Web.Helpers;
using ClienteWebEurekaBank.Web.Interfaces;
using ClienteWebEurekaBank.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClienteWebEurekaBank.Web.Controllers;

[ExigirSesion]
public sealed class OperacionesController : Controller
{
    private readonly IApiEurekaServicio _apiEurekaServicio;

    public OperacionesController(IApiEurekaServicio apiEurekaServicio)
    {
        _apiEurekaServicio = apiEurekaServicio;
    }

    [HttpGet]
    public IActionResult Deposito()
    {
        return View("/Vistas/Operaciones/Deposito.cshtml", new DepositoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposito(DepositoViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Operaciones/Deposito.cshtml", modelo);
        }

        var respuesta = await _apiEurekaServicio.RegistrarDepositoAsync(modelo.Cuenta, modelo.Importe!.Value, cancellationToken);
        if (!respuesta.EsExitoso)
        {
            TempData["MensajeError"] = respuesta.Mensaje;
            return View("/Vistas/Operaciones/Deposito.cshtml", modelo);
        }

        TempData["MensajeExito"] = respuesta.Mensaje;
        return RedirectToAction(nameof(Deposito));
    }

    [HttpGet]
    public IActionResult Retiro()
    {
        return View("/Vistas/Operaciones/Retiro.cshtml", new RetiroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Retiro(RetiroViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Operaciones/Retiro.cshtml", modelo);
        }

        var respuesta = await _apiEurekaServicio.RegistrarRetiroAsync(modelo.Cuenta, modelo.Importe!.Value, cancellationToken);
        if (!respuesta.EsExitoso)
        {
            TempData["MensajeError"] = respuesta.Mensaje;
            return View("/Vistas/Operaciones/Retiro.cshtml", modelo);
        }

        TempData["MensajeExito"] = respuesta.Mensaje;
        return RedirectToAction(nameof(Retiro));
    }

    [HttpGet]
    public IActionResult Transferencia()
    {
        return View("/Vistas/Operaciones/Transferencia.cshtml", new TransferenciaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Transferencia(TransferenciaViewModel modelo, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("/Vistas/Operaciones/Transferencia.cshtml", modelo);
        }

        if (string.Equals(modelo.CuentaOrigen, modelo.CuentaDestino, StringComparison.Ordinal))
        {
            ModelState.AddModelError(string.Empty, "La cuenta origen y la cuenta destino no pueden ser iguales.");
            return View("/Vistas/Operaciones/Transferencia.cshtml", modelo);
        }

        var respuesta = await _apiEurekaServicio.RegistrarTransferenciaAsync(
            modelo.CuentaOrigen,
            modelo.CuentaDestino,
            modelo.Importe!.Value,
            cancellationToken);

        if (!respuesta.EsExitoso)
        {
            TempData["MensajeError"] = respuesta.Mensaje;
            return View("/Vistas/Operaciones/Transferencia.cshtml", modelo);
        }

        TempData["MensajeExito"] = respuesta.Mensaje;
        return RedirectToAction(nameof(Transferencia));
    }
}
