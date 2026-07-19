using ClienteWebEurekaBank.Web.DTOs;

namespace ClienteWebEurekaBank.Web.Interfaces;

public interface IApiEurekaServicio
{
    Task<RespuestaApiDto> ValidarUsuarioAsync(string usuario, string password, CancellationToken cancellationToken = default);
    Task<(RespuestaApiDto Respuesta, List<MovimientoDto> Movimientos)> ObtenerMovimientosAsync(string cuenta, CancellationToken cancellationToken = default);
    Task<RespuestaApiDto> RegistrarDepositoAsync(string cuenta, decimal importe, CancellationToken cancellationToken = default);
    Task<RespuestaApiDto> RegistrarRetiroAsync(string cuenta, decimal importe, CancellationToken cancellationToken = default);
    Task<RespuestaApiDto> RegistrarTransferenciaAsync(string cuentaOrigen, string cuentaDestino, decimal importe, CancellationToken cancellationToken = default);
}
