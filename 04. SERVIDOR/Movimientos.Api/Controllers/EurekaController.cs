using Microsoft.AspNetCore.Mvc;
using Movimientos.Api.Servicio;

namespace Movimientos.Api.Controllers
{
    [ApiController]
    [Route("Eureka")]
    public class EurekaController : ControllerBase
    {
        private readonly MovimientosServicio _movimientosServicio;

        public EurekaController()
        {
            _movimientosServicio = new MovimientosServicio();
        }

        /// <summary>
        /// Obtiene los movimientos de una cuenta.
        /// </summary>
        /// <param name="cuenta">Código de la cuenta.</param>
        /// <returns>Lista de movimientos.</returns>
        [HttpGet("movimientos/{cuenta}")]
        public IActionResult LeerMovimientos(string cuenta)
        {
            try
            {
                var movimientos = _movimientosServicio.LeerMovimientos(cuenta);
                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al leer movimientos: {ex.Message}");
            }
        }
    }
}
