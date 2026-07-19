using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Text.Json;

namespace WS_Eurekabank_CLICON.ec.edu.monster.modelo
{

    public class EurekabankService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUri = "http://localhost:5069/Eureka";

        public EurekabankService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> Login(string usuario, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                {
                    return "Error: Usuario y contraseña requeridos.";
                }

                var uri = $"{BaseUri}/validar-usuario?usuario={Uri.EscapeDataString(usuario)}&password={Uri.EscapeDataString(password)}";
                var response = await _httpClient.PostAsync(uri, null);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return content;
                }
                else
                {
                    return $"Error: {content}";
                }
            }
            catch (HttpRequestException ex)
            {
                return $"Error de conexión: {ex.Message}";
            }
            catch (TaskCanceledException ex)
            {
                return $"Tiempo de espera agotado: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"Error inesperado: {ex.Message}";
            }
        }

        public async Task<List<Movimiento>> ObtenerMovimientos(string cuenta)
        {
            var uri = $"{BaseUri}/movimientos/{cuenta}";
            var response = await _httpClient.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var movimientos = JsonSerializer.Deserialize<List<Movimiento>>(content);
                    return movimientos ?? new List<Movimiento>();
                }
                catch
                {
                    Console.WriteLine("Error al procesar los movimientos.");
                    return new List<Movimiento>();
                }
            }
            else
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
                    Console.WriteLine($"Error: {errorResponse.error}");
                }
                catch
                {
                    Console.WriteLine("Error desconocido al obtener los movimientos.");
                }

                return new List<Movimiento>();
            }
        }


        public async Task<string> RegistrarDeposito(string cuenta, double importe)
        {
            var uri = $"{BaseUri}/deposito?cuenta={cuenta}&importe={importe}";
            var response = await _httpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var depositoResponse = JsonSerializer.Deserialize<OperacionResponse>(content);
                    return $"Estado: {depositoResponse.estado}, Mensaje: {depositoResponse.mensaje}";
                }
                catch
                {
                    return content; // Return raw content if deserialization fails
                }
            }
            else
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
                    return $"Error: {errorResponse.error}";
                }
                catch
                {
                    return content; // Return raw content if deserialization fails
                }
            }
        }

        public async Task<string> RegistrarTransferencia(string cuentaOrigen, string cuentaDestino, double importe)
        {
            var uri = $"{BaseUri}/transferencia?cuentaOrigen={cuentaOrigen}&cuentaDestino={cuentaDestino}&importe={importe}";
            var response = await _httpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var transferenciaResponse = JsonSerializer.Deserialize<OperacionResponse>(content);
                    return $"Estado: {transferenciaResponse.estado}, Mensaje: {transferenciaResponse.mensaje}";
                }
                catch
                {
                    return content;
                }
            }

            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
                return $"Error: {errorResponse.error}";
            }
            catch
            {
                return content;
            }
        }

        public async Task<string> RegistrarRetiro(string cuenta, double importe)
        {
            var uri = $"{BaseUri}/retiro?cuenta={cuenta}&importe={importe}";
            var response = await _httpClient.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var retiroResponse = JsonSerializer.Deserialize<OperacionResponse>(content);
                    return $"Estado: {retiroResponse.estado}, Mensaje: {retiroResponse.mensaje}";
                }
                catch
                {
                    return content; // Return raw content if deserialization fails
                }
            }
            else
            {
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
                    return $"Error: {errorResponse.error}";
                }
                catch
                {
                    return content; // Return raw content if deserialization fails
                }
            }
        }
    }
}
