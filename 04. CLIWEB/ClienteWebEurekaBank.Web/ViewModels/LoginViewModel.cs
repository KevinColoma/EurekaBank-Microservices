using System.ComponentModel.DataAnnotations;

namespace ClienteWebEurekaBank.Web.ViewModels;

public sealed class LoginViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    [Display(Name = "Usuario")]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "La password es obligatoria.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}
