using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CMS.Model.Interface;

namespace CMS.Helper
{
    public class JwtHelper
    {
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
        public List<System.Security.Claims.Claim> GetClaims(string? authHeader)
        {
            if (string.IsNullOrEmpty(authHeader))
                return null;
            List<System.Security.Claims.Claim> claims;
            var _jwtHelper = new JwtHelper();
            var jsonToken = _jwtHelper.Decode(authHeader) as JwtSecurityToken;
            claims = jsonToken.Claims.ToList();

            var expireDate = DateTime.Parse(claims.First(x => x.Type == "ExpireDate").Value);
            if (expireDate < DateTime.Now)
                return null;

            return claims;
        }
    }
}
