using CMS.Helper;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.ComponentModel;
using System.Security.Claims;

namespace CMS.Tools;

public class RequestInfo : CMS.Model.Interface.IRequestInfo
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    public RequestInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        if (_httpContextAccessor.HttpContext?.Request != null && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            claims = new JwtHelper().GetClaims(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]);
        else
            claims = new List<Claim>();
    }
    public List<Claim> claims { get; private set; }

    public string? UserName => claims.FirstOrDefault(x => x.Type == "UserName")?.Value;


}
