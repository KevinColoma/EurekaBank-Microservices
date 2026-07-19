using System.Net;
using System.Text.Json;
using ClienteWebEurekaBank.Web.DTOs;
using ClienteWebEurekaBank.Web.Interfaces;

namespace ClienteWebEurekaBank.Web.Servicios;

public sealed class ApiEurekaServicio : IApiEurekaServicio
{
    private static readonly JsonSerializerOptions OpcionesJson = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _clienteHttp;

    public ApiEurekaServicio(HttpClient clienteHttp)
    {
        _clienteHttp = clienteHttp;
    }

    public async Task<RespuestaApiDto> ValidarUsuarioAsync(string usuario, string password, CancellationToken cancellationToken = default)
    {
        var url = $"validar-usuario?usuario={Uri.EscapeDataString(usuario)}&password={Uri.EscapeDataString(password)}";
        using var solicitud = new HttpRequestMessage(HttpMethod.Post, url);

        try
        {
            using var respuesta = await _clienteHttp.SendAsync(solicitud, cancellationToken);
            var contenido = await LeerContenidoComoTextoAsync(respuesta, cancellationToken);

            if (respuesta.IsSuccessStatusCode)
            {
                return new RespuestaApiDto { EsExitoso = true, Mensaje = ExtraerMensajePlano(contenido) };
            }

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new RespuestaApiDto { EsExitoso = false, Mensaje = "Credenciales incorrectas.", CodigoEstadoHttp = (int)respuesta.StatusCode };
            }

            return new RespuestaApiDto
            {
                EsExitoso = false,
                Mensaje = ExtraerMensajePlano(contenido),
                CodigoEstadoHttp = (int)respuesta.StatusCode
            };
        }
        catch (TaskCanceledException)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "Tiempo de espera agotado al conectar con la API." };
        }
        catch (HttpRequestException)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "No se pudo conectar con la API." };
        }
        catch (Exception)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "Ocurrió un error inesperado al validar el usuario." };
        }
    }

    public async Task<(RespuestaApiDto Respuesta, List<MovimientoDto> Movimientos)> ObtenerMovimientosAsync(string cuenta, CancellationToken cancellationToken = default)
    {
        var url = $"movimientos/{Uri.EscapeDataString(cuenta)}";

        try
        {
            using var respuesta = await _clienteHttp.GetAsync(url, cancellationToken);
            var contenido = await LeerContenidoComoTextoAsync(respuesta, cancellationToken);

            if (respuesta.IsSuccessStatusCode)
            {
                var movimientos = JsonSerializer.Deserialize<List<MovimientoDto>>(contenido, OpcionesJson) ?? new();
                return (new RespuestaApiDto { EsExitoso = true, Mensaje = "Consulta realizada correctamente." }, movimientos);
            }

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                return (new RespuestaApiDto { EsExitoso = false, Mensaje = "No autorizado.", CodigoEstadoHttp = (int)respuesta.StatusCode }, new());
            }

            return (new RespuestaApiDto { EsExitoso = false, Mensaje = ExtraerMensajePlano(contenido), CodigoEstadoHttp = (int)respuesta.StatusCode }, new());
        }
        catch (TaskCanceledException)
        {
            return (new RespuestaApiDto { EsExitoso = false, Mensaje = "Tiempo de espera agotado al consultar movimientos." }, new());
        }
        catch (HttpRequestException)
        {
            return (new RespuestaApiDto { EsExitoso = false, Mensaje = "No se pudo conectar con la API." }, new());
        }
        catch (JsonException)
        {
            return (new RespuestaApiDto { EsExitoso = false, Mensaje = "La API devolvió un formato de datos inesperado." }, new());
        }
        catch (Exception)
        {
            return (new RespuestaApiDto { EsExitoso = false, Mensaje = "Ocurrió un error inesperado al consultar movimientos." }, new());
        }
    }

    public Task<RespuestaApiDto> RegistrarDepositoAsync(string cuenta, decimal importe, CancellationToken cancellationToken = default)
    {
        var url = $"deposito?cuenta={Uri.EscapeDataString(cuenta)}&importe={importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        return EjecutarOperacionMonetariaAsync(url, "Depósito registrado correctamente.", cancellationToken);
    }

    public Task<RespuestaApiDto> RegistrarRetiroAsync(string cuenta, decimal importe, CancellationToken cancellationToken = default)
    {
        var url = $"retiro?cuenta={Uri.EscapeDataString(cuenta)}&importe={importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        return EjecutarOperacionMonetariaAsync(url, "Retiro registrado correctamente.", cancellationToken);
    }

    public Task<RespuestaApiDto> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, decimal importe, CancellationToken cancellationToken = default)
    {
        var url = $"transferencia?cuentaOrigen={Uri.EscapeDataString(cuentaOrigen)}&cuentaDestino={Uri.EscapeDataString(cuentaDestino)}&importe={importe.ToString(System.Globalization.CultureInfo.InvariantCulture)}";
        return EjecutarOperacionMonetariaAsync(url, "Transferencia registrada correctamente.", cancellationToken);
    }

    private async Task<RespuestaApiDto> EjecutarOperacionMonetariaAsync(string url, string mensajeExito, CancellationToken cancellationToken)
    {
        using var solicitud = new HttpRequestMessage(HttpMethod.Post, url);

        try
        {
            using var respuesta = await _clienteHttp.SendAsync(solicitud, cancellationToken);
            var contenido = await LeerContenidoComoTextoAsync(respuesta, cancellationToken);

            if (respuesta.IsSuccessStatusCode)
            {
                return new RespuestaApiDto { EsExitoso = true, Mensaje = string.IsNullOrWhiteSpace(contenido) ? mensajeExito : ExtraerMensajePlano(contenido) };
            }

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new RespuestaApiDto { EsExitoso = false, Mensaje = "No autorizado.", CodigoEstadoHttp = (int)respuesta.StatusCode };
            }

            return new RespuestaApiDto { EsExitoso = false, Mensaje = ExtraerMensajePlano(contenido), CodigoEstadoHttp = (int)respuesta.StatusCode };
        }
        catch (TaskCanceledException)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "Tiempo de espera agotado al registrar la operación." };
        }
        catch (HttpRequestException)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "No se pudo conectar con la API." };
        }
        catch (Exception)
        {
            return new RespuestaApiDto { EsExitoso = false, Mensaje = "Ocurrió un error inesperado al registrar la operación." };
        }
    }

    private static async Task<string> LeerContenidoComoTextoAsync(HttpResponseMessage respuesta, CancellationToken cancellationToken)
    {
        if (respuesta.Content is null)
        {
            return string.Empty;
        }

        return await respuesta.Content.ReadAsStringAsync(cancellationToken);
    }

    private static string ExtraerMensajePlano(string? contenido)
    {
        if (string.IsNullOrWhiteSpace(contenido))
        {
            return "La API no devolvió un mensaje.";
        }

        var texto = contenido.Trim();
        if (texto.Length >= 2 && texto.StartsWith('"') && texto.EndsWith('"'))
        {
            return texto[1..^1];
        }

        return texto;
    }
}
