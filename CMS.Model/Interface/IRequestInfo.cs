using System.Security.Claims;

namespace CMS.Model.Interface;

public interface IRequestInfo
{
    public List<Claim> claims { get; }

    public string? UserName { get; }


}
