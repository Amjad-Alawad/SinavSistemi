using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class PersonelFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        var rol = httpContext.Session.GetString("Rol");

        if (rol != "Personel")
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }

        base.OnActionExecuting(context);
    }
}