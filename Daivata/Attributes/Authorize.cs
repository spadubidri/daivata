using System.Web.Mvc;

namespace Daivata.UI
{
    public class AuthorizeAccess : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (actionContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.OnActionExecuting(actionContext);
            }
            else
            {
                actionContext.HttpContext.Response.Redirect("~/Security/Login");
            }
 
        }
    }
}