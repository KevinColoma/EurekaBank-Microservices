using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClienteWebEurekaBank.Web.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class ExigirSesionAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext contexto)
    {
        var usuario = contexto.HttpContext.Session.GetString(ConstantesSesion.UsuarioAutenticado);
        if (string.IsNullOrWhiteSpace(usuario))
        {
            contexto.Result = new RedirectToActionResult("IniciarSesion", "Acceso", null);
        }
    }
}
