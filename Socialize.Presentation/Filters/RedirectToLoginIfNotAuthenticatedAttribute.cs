using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Socialize.Presentation.Filters
{
    public class RedirectToLoginIfNotAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verificar si el usuario está autenticado
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                // Agregar mensaje de error a TempData
                var tempData = context.HttpContext.RequestServices.GetService<ITempDataDictionaryFactory>().GetTempData(context.HttpContext);
                tempData["ErrorMessage"] = "You must sign in to have access to these features.";

                // Redirigir al inicio de sesión con la URL de retorno
                string returnUrl = context.HttpContext.Request.Path;
                context.Result = new RedirectToActionResult("Index", "Home", new { ReturnUrl = returnUrl });
            }

            base.OnActionExecuting(context);
        }
    }

}
