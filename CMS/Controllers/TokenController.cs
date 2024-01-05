using CMS.Model;
using CMS.Model.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace CMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        [Tools.Authorize, HttpPost, Route("Get2")]
        public async Task<Result<TokenObject>> Get2(User model)
        {

            return Result<TokenObject>.Successful();
        }

        [HttpPost, Route("Get")]
        public async Task<Result<TokenObject>> Get(User model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                return Result<TokenObject>.Failure(message: "نام کاربری و رمز عبور را وارد کنید");

            var dataSource = new Dal.DataSource.UserDataSource();
            var userResult = await dataSource.GetByuserPassAsync(model.UserName, model.Password);
            if (!userResult.Success)
                return Result<TokenObject>.Failure(message: userResult.Message);
            if (userResult.Data == null)
                return Result<TokenObject>.Failure(message: "کاربر یافت نشد");


            var resultClaims = await getClaims(userResult.Data);
            if (!resultClaims.Success)
                return Result<TokenObject>.Failure(message: resultClaims.Message);
            var claims = resultClaims.Data;

            var expireDate = DateTime.Now.AddHours(Property.AccessTokenExpireTimeSpan);
            var issuedDate = DateTime.Now;

            var token = GetToken(claims, issuedDate, expireDate);

            //var ds2 = claims2.Where(x => x.Type == "RefreshTokenID").ToList();

            return Result<TokenObject>.Successful(data: token);
        }
        private async Task<Result<List<Claim>>> getClaims(User user)
        {
            List<Claim> claims;

            claims = new List<Claim>
                {
                    new Claim(type: "UserName", value: user.UserName??""),
                    new Claim(type: "Type", value: user.Type.ToString()),
                    new Claim(type: "UserId", value: user.Id.ToString()),
                };
            //var returnObj = Tuple.Create(defaultPosition, claims);

            return Result<List<Claim>>.Successful(data: claims);
        }
        private TokenObject GetToken(List<Claim> claims, DateTime issuedDate, DateTime expireDate)
        {
            var token = new Helper.JwtHelper().Code(claims, expireDate);

            var tokenObject = new TokenObject
            {
                Expires = expireDate.ToString(),
                Issued = issuedDate.ToString(),
                AccessToken = token,
                TokenType = "bearer",
            };

            return tokenObject;
        }
    }
}
