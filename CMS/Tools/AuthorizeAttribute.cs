using CMS.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CMS.Tools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        if (checkAuthorize(context.ActionDescriptor as ControllerActionDescriptor))
        {
            string? authHeader = context.HttpContext.Request.Headers["Authorization"];
            var claims = new JwtHelper().GetClaims(authHeader);
            if (claims == null)
            {
                context.Result = new UnauthorizedResult();
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
