using System.ComponentModel.DataAnnotations;

namespace ClienteWebEurekaBank.Web.ViewModels;

public sealed class TransferenciaViewModel
{
    [Required(ErrorMessage = "La cuenta origen es obligatoria.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "La cuenta origen debe tener 8 caracteres.")]
    [Display(Name = "Cuenta origen")]
    public string CuentaOrigen { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cuenta destino es obligatoria.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "La cuenta destino debe tener 8 caracteres.")]
    [Display(Name = "Cuenta destino")]
    public string CuentaDestino { get; set; } = string.Empty;

    [Required(ErrorMessage = "El importe es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0.")]
    [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "El importe debe tener como máximo 2 decimales.")]
    [Display(Name = "Importe")]
    public decimal? Importe { get; set; }
}
