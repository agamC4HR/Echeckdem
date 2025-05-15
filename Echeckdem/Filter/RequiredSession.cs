using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Echeckdem.Filter
{
    public class RequiredSession:IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var routeData = context.ActionDescriptor.RouteValues;

            var controller = routeData["controller"]?.ToLower();
            var action = routeData["action"]?.ToLower();

            // Allow access to Login/Index without session
            if (controller == "login" && action == "index")
                return;


            if (session.GetInt32("User Level") == null)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
