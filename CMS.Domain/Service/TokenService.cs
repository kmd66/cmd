using CMS.Model;
using CMS.Model.Interface;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Domain.Service
{
    public class TokenService : BaseService, IService
    {
        public TokenService() {
        }


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
            var token = Code(claims, expireDate);

            var tokenObject = new TokenObject
            {
                Expires = expireDate.ToString(),
                Issued = issuedDate.ToString(),
                AccessToken = token,
                TokenType = "bearer",
            };

            return tokenObject;
        }
        public string Code(List<Claim> claims, DateTime expires)
        {
            IConfiguration _configuration = IContainer.Instance.GetService<IConfiguration>();

            var jwtSecret = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Jwt:Secret"]));
            var ewtKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Jwt:EwtKey"]));

            claims.Add(new Claim(type: "ExpireDate", value: expires.ToString()));
            //var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_APISettings.SecretKey));
            var signingCredentials = new SigningCredentials(jwtSecret, SecurityAlgorithms.HmacSha256);
            var encryptionCredentials = new EncryptingCredentials(ewtKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);
            var tokenOptions = new JwtSecurityTokenHandler().CreateJwtSecurityToken(new SecurityTokenDescriptor()
            {
                Audience = "-",
                Issuer = "_",
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                EncryptingCredentials = encryptionCredentials,
                SigningCredentials = signingCredentials
            });
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }
        public dynamic Decode(string token)
        {
            IConfiguration _configuration = IContainer.Instance.GetService<IConfiguration>();

            var jwtSecret = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Jwt:Secret"]));
            var ewtKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_configuration["Jwt:EwtKey"]));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = "-",
                ValidAudience = "-",
                ValidateIssuerSigningKey = false,
                ValidateLifetime = false,
                RequireExpirationTime = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = jwtSecret,
                TokenDecryptionKey = ewtKey,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;

            return validatedToken;
        }
    }
}

