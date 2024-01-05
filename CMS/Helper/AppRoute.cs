using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CMS.Model.Interface;

namespace CMS.Helper
{
    public static class AppRoute
    {
        // token
        public const string TokenGet = "api/Token/Get";
        public const string TokenGet2 = "api/Token/Get2";

    }
}
