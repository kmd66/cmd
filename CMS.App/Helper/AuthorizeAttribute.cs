using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;

namespace CMS.App.Helper
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            if (checkAuthorize(context.ActionDescriptor as ControllerActionDescriptor))
            {
                var cooki = context.HttpContext.Request.Cookies["token"];
                var claims = new Helper.JwtHelper().GetClaims(cooki);
                if (claims == null)
                {
                    context.Result = new RedirectResult("/cmd-login");
                    return;
                }
            }
        }

        private bool checkAuthorize(ControllerActionDescriptor controllerActionDescriptor)
        {
            if (controllerActionDescriptor != null)
            {
                var ControllerChecking = controllerActionDescriptor.ControllerTypeInfo.CustomAttributes.Where(w => w.AttributeType.Name.Contains("AllowAnonymous")).ToList();
                if (ControllerChecking.Count > 0)
                    return false;
                var ActionChecking = controllerActionDescriptor.MethodInfo.CustomAttributes.Where(w => w.AttributeType.Name.Contains("AllowAnonymous")).ToList();
                if (ActionChecking.Count > 0)
                    return false;
            }

            return true;
        }

    }
}
