using Microsoft.AspNetCore.Components;
using CMS.Model;
using System.Net;
using CMS.Dal.DataSource;

namespace CMS.Helper
{
    public class BaseHelper
    {
        public BaseHelper()
        {
            isAuthorize = true;
        }
        public BaseHelper(bool auth)
        {
            isAuthorize = auth;
        }
        public BaseHelper(string? auth)
        {
            if (!string.IsNullOrEmpty(auth))
            {
                var claims = new JwtHelper().GetClaims(auth);
                if (claims != null)
                    isAuthorize = true;
            }
        }
        public BaseHelper(string? auth, string? _remoteIpAddress)
        {
            if (auth != null)
            {
                var claims = new JwtHelper().GetClaims(auth);
                if (claims != null)
                    isAuthorize = true;
            }
            remoteIpAddress = _remoteIpAddress;
        }
        public string? remoteIpAddress;
        public bool isAuthorize { get; set; }

    }
}
