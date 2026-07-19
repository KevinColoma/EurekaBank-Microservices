using Microsoft.AspNetCore.Mvc;
using Auth.Api.Servicio;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("Eureka")]
    public class EurekaController : ControllerBase
    {
        private readonly AuthServicio _authService;

        public EurekaController()
        {
            _authService = new AuthServicio();
        }

        /// <summary>
        /// Valida las credenciales de un usuario.
        /// </summary>
        /// <param name="usuario">Nombre de usuario.</param>
        /// <param name="password">Contraseña.</param>
        /// <returns>Resultado de la validación.</returns>
        [HttpPost("validar-usuario")]
        public IActionResult ValidarUsuario([FromQuery] string usuario, [FromQuery] string password)
        {
            try
            {
                var esValido = _authService.ValidarUsuario(usuario, password);
                if (esValido)
                    return Ok("Usuario válido.");
                else
                    return Unauthorized("Usuario o contraseña incorrectos.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al validar usuario: {ex.Message}");
            }
        }
    }
}
