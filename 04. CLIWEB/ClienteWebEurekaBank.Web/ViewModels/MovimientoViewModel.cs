using System.ComponentModel.DataAnnotations;
using ClienteWebEurekaBank.Web.DTOs;

namespace ClienteWebEurekaBank.Web.ViewModels;

public sealed class MovimientoViewModel
{
    [Required(ErrorMessage = "La cuenta es obligatoria.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "La cuenta debe tener 8 caracteres.")]
    [Display(Name = "Cuenta")]
    public string Cuenta { get; set; } = string.Empty;

    public List<MovimientoDto> Movimientos { get; set; } = new();
}
