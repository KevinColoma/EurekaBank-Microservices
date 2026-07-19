using System.Text.Json.Serialization;

namespace ClienteWebEurekaBank.Web.DTOs;

public sealed class MovimientoDto
{
    [JsonPropertyName("cuenta")]
    public string Cuenta { get; set; } = string.Empty;

    [JsonPropertyName("nromov")]
    public int NumeroMovimiento { get; set; }

    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonPropertyName("accion")]
    public string Accion { get; set; } = string.Empty;

    [JsonPropertyName("importe")]
    public decimal Importe { get; set; }
}
