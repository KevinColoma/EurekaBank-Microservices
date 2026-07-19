namespace ClienteWebEurekaBank.Web.DTOs;

public sealed class RespuestaApiDto
{
    public bool EsExitoso { get; set; }
    public string Mensaje { get; set; } = string.Empty;
    public int? CodigoEstadoHttp { get; set; }
}
