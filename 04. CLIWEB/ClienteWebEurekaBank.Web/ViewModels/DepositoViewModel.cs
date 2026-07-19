using System.ComponentModel.DataAnnotations;

namespace ClienteWebEurekaBank.Web.ViewModels;

public sealed class DepositoViewModel
{
    [Required(ErrorMessage = "La cuenta es obligatoria.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "La cuenta debe tener 8 caracteres.")]
    [Display(Name = "Cuenta")]
    public string Cuenta { get; set; } = string.Empty;

    [Required(ErrorMessage = "El importe es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor a 0.")]
    [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "El importe debe tener como máximo 2 decimales.")]
    [Display(Name = "Importe")]
    public decimal? Importe { get; set; }
}
