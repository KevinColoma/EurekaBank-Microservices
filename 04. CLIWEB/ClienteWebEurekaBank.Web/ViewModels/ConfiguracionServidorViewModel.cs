using System.ComponentModel.DataAnnotations;

namespace ClienteWebEurekaBank.Web.ViewModels;

public sealed class ConfiguracionServidorViewModel
{
    [Required(ErrorMessage = "La URL del servidor es obligatoria.")]
    [Display(Name = "URL base de la API")]
    public string UrlBase { get; set; } = string.Empty;
}
