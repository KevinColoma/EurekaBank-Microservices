using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model;
using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.controller
{
    public class LoginController
    {
        // Instancia del servicio que habla con la API
        private EurekaService _eurekaService = new EurekaService();

        /// <summary>
        /// Autentica al usuario de forma asincrónica.
        /// </summary>
        /// <param name="model">El ViewModel con los datos de la UI</param>
        /// <returns>True si es exitoso, False si falla.</returns>
        public async Task<bool> AutenticarAsync(LoginViewModel model)
        {
            // 1. Mapear del ViewModel (UI) al DTO (API Request)
            var usuario = new Usuario
            {
                NombreUsuario = model.Usuario,
                Contrasena = model.Contrasena
            };

            try
            {
                // 2. Llamar al servicio
                bool resultado = await _eurekaService.LoginAsync(usuario);

                if (resultado)
                {
                    return true;
                }
                else
                {
                    // 3. Manejar error de credenciales
                    model.MensajeError = "Usuario o contraseña inválidos.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                // 4. Manejar error de conexión
                // (Captura el error de conexión propagado por el servicio)
                string errorBase = ex.InnerException?.Message ?? ex.Message;
                model.MensajeError = "Error de conexión con el servicio: " + errorBase;
                return false;
            }
        }
    }
}
