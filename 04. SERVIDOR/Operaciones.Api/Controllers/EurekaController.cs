using Microsoft.AspNetCore.Mvc;
using Operaciones.Api.Servicio;

namespace Operaciones.Api.Controllers
{
    [ApiController]
    [Route("Eureka")]
    public class EurekaController : ControllerBase
    {
        private readonly OperacionesServicio _operacionesServicio;

        public EurekaController()
        {
            _operacionesServicio = new OperacionesServicio();
        }

        /// <summary>
        /// Registra un depósito en una cuenta.
        /// </summary>
        /// <param name="cuenta">Código de la cuenta.</param>
        /// <param name="importe">Importe del depósito.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpPost("deposito")]
        public IActionResult RegistrarDeposito([FromQuery] string cuenta, [FromQuery] double importe)
        {
            string codEmp = "0001";
            try
            {
                var estado = _operacionesServicio.RegistrarDeposito(cuenta, importe, codEmp);
                if (estado == 1)
                    return Ok("Depósito registrado con éxito.");
                else
                    return BadRequest("La cuenta no existe o no está activa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar depósito: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra un retiro en una cuenta.
        /// </summary>
        /// <param name="cuenta">Código de la cuenta.</param>
        /// <param name="importe">Importe del retiro.</param>
        /// <returns>Estado de la operación.</returns>
        [HttpPost("retiro")]
        public IActionResult RegistrarRetiro([FromQuery] string cuenta, [FromQuery] double importe)
        {
            string codEmp = "0001";
            try
            {
                var estado = _operacionesServicio.RegistrarRetiro(cuenta, importe, codEmp);
                if (estado == 1)
                    return Ok("Retiro registrado con éxito.");
                else if (estado == -2)
                    return BadRequest("Saldo insuficiente.");
                else
                    return BadRequest("La cuenta no existe o no está activa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al registrar retiro: {ex.Message}");
            }
        }
    }
}
