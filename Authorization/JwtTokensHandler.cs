

using System.Text;
using ConfidentVotesAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ConfidentVotesAPI;
using Microsoft.AspNetCore.Identity;

namespace ConfidentVoteAPI.Authentication
{
   public interface IJwtTokensHandler
    {
        public  string GenerateBearerJwt(string email, string UserId, List<string> roles, 
                         DateTime expirationDate,
                        string secretKey, string jwtIssuer = "builderIssuser", 
                        string JwtAudience = "builderAudience"
                         );
    }
   public class JwtTokensHandler : IJwtTokensHandler
    {
         public  string GenerateBearerJwt(string username, string UserId, List<string> roles, 
                         DateTime expirationDate,
                        string secretKey, string jwtIssuer = "builderIssuser", 
                        string JwtAudience = "builderAudience"
                         )
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
             {
                new Claim(JwtRegisteredClaimNames.Sub, UserId),
                new Claim(JwtRegisteredClaimNames.Jti, username),
             };
            foreach ( var role in roles)
            {
                claims.Add( new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: JwtAudience,
                claims: claims,
                expires: expirationDate,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}