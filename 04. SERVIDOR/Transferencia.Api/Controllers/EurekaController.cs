using Microsoft.AspNetCore.Mvc;
using Transferencia.Api.Servicio;

namespace Transferencia.Api.Controllers
{
    [ApiController]
    [Route("Eureka")]
    public class EurekaController : ControllerBase
    {
        private readonly TransferenciaServicio _transferenciaService;

        public EurekaController()
        {
            _transferenciaService = new TransferenciaServicio();
        }

        /// <summary>
        /// Registra una transferencia entre cuentas.
        /// </summary>
        /// <param name="cuentaOrigen">Código de la cuenta origen.</param>
        /// <param name="cuentaDestino">Código de la cuenta destino.</param>
        /// <param name="importe">Importe de la transferencia.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpPost("transferencia")]
        public IActionResult RegistrarTransferencia([FromQuery] string cuentaOrigen, [FromQuery] string cuentaDestino, [FromQuery] double importe)
        {
            string codEmp = "0001";
            try
            {
                var estado = _transferenciaService.RegistrarTransferencia(cuentaOrigen, cuentaDestino, importe, codEmp);
                if (estado == 1)
                    return Ok("Transferencia realizada con éxito.");
                else if (estado == -2)
                    return BadRequest("Saldo insuficiente en la cuenta origen.");
                else if (estado == -3)
                    return BadRequest("La cuenta destino no existe o no está activa.");
                else
                    return BadRequest("La cuenta origen no existe o no está activa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar transferencia: {ex.Message}");
            }
        }
    }
}
