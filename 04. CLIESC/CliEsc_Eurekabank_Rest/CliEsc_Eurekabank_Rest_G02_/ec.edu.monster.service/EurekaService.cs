using CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CliEsc_Eurekabank_Rest_G02_.ec.edu.monster.service
{
    public class EurekaService
    {
        // Mejor práctica: Usar un HttpClient estático para evitar
        // el agotamiento de sockets en aplicaciones de larga duración (como WinForms).
        // La URL base ya no es fija: se arma en cada llamada con Config.BaseUrl, de modo
        // que la IP del servidor puede cambiarse desde la interfaz sin recompilar.
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 1. Autentica al usuario (POST /validar-usuario)
        /// </summary>
        /// <param name="usuario">Modelo 'Usuario' con NombreUsuario y Contrasena</param>
        public async Task<bool> LoginAsync(Usuario usuario)
        {
            try
            {
                // Usa parámetros query en lugar de JSON body
                var url = Config.BaseUrl + $"validar-usuario?usuario={Uri.EscapeDataString(usuario.NombreUsuario)}&password={Uri.EscapeDataString(usuario.Contrasena)}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return false; // Autenticación fallida (401)
                }

                response.EnsureSuccessStatusCode(); // Lanza error para otros 4xx/5xx

                return true; // La API devuelve un mensaje de éxito
            }
            catch (Exception ex)
            {
                // Propaga la excepción para que el 'Controller' la maneje
                throw new Exception("Error al conectar o procesar el servicio de login.", ex);
            }
        }

        /// <summary>
        /// 2. Trae Movimientos (GET /movimientos/{numCuenta})
        /// </summary>
        public async Task<List<Movimiento>> TraerMovimientoAsync(string numCuenta)
        {
            try
            {
                var response = await _httpClient.GetAsync(Config.BaseUrl + $"movimientos/{numCuenta}");

                if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return new List<Movimiento>(); // Error o sin movimientos
                }

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Movimiento>>(responseBody);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener movimientos para la cuenta {numCuenta}.", ex);
            }
        }

        /// <summary>
        /// 3. Registra Depósito (POST /deposito)
        /// </summary>
        public async Task<string> RegDepositoAsync(DepositoRequest request)
        {
            try
            {
                var url = Config.BaseUrl + $"deposito?cuenta={Uri.EscapeDataString(request.NumCuenta)}&importe={request.Importe}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMsg); // Error de negocio
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el depósito.", ex);
            }
        }

        /// <summary>
        /// 4. Registra Retiro (POST /retiro)
        /// </summary>
        public async Task<string> RegRetiroAsync(DepositoRequest request)
        {
            try
            {
                var url = Config.BaseUrl + $"retiro?cuenta={Uri.EscapeDataString(request.NumCuenta)}&importe={request.Importe}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMsg); // Error de negocio
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el retiro.", ex);
            }
        }

        /// <summary>
        /// 5. Registra Transferencia (POST /transferencia)
        /// </summary>
        public async Task<string> RegTransferenciaAsync(TransferenciaRequest request)
        {
            try
            {
                var url = Config.BaseUrl + $"transferencia?cuentaOrigen={Uri.EscapeDataString(request.CuentaOrigen)}&cuentaDestino={Uri.EscapeDataString(request.CuentaDestino)}&importe={request.Importe}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception(errorMsg); // Error de negocio
                }

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la transferencia.", ex);
            }
        }
    }
}
