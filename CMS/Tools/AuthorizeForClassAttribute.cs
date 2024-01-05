using CMS.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CMS.Tools;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeForClassAttribute : Attribute
{
    public AuthorizeForClassAttribute(bool isAuthorize)
    {
        this.isAuthorize = isAuthorize;
    }
    private bool isAuthorize { get; set; }

}
