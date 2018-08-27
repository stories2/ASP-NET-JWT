using ASP_NET_JWT.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ASP_NET_JWT.Utils
{
    public class TokenManager
    {
        private static string privateKey = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";

        public static string GenerateToken(string userName)
        {
            byte[] privateByteKey = Convert.FromBase64String(privateKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(privateByteKey);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),

                Expires = DateTime.UtcNow.AddMinutes(DefineManager.EXPIRE_TOKEN_TIME_MIN),

                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);

            //Custom value
            //jwtToken.Payload["terran"] = "not enough minerals";

            return jwtTokenHandler.WriteToken(jwtToken);
        }

        public static string ValidateToken(string token)
        {
            string userName = null;
            ClaimsIdentity identity = null;

            ClaimsPrincipal principal = GetClaimsPrincipal(token);
            if(principal == null)
            {
                return null;
            }

            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch(Exception err)
            {
                return null;
            }

            Claim userNameClaim = identity.FindFirst(ClaimTypes.Name); //line 24
            userName = userNameClaim.Value;
            return userName;
        }

        static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)jwtTokenHandler.ReadToken(token);

                if(jwtToken == null)
                {
                    return null;
                }

                byte[] privateByteKey = Convert.FromBase64String(privateKey);
                TokenValidationParameters validationParams = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(privateByteKey)
                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = jwtTokenHandler.ValidateToken(token, validationParams, out securityToken);
                return principal;
            }
            catch(Exception err)
            {
                return null;
            }
        }
    }
}