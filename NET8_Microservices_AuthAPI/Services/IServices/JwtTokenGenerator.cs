using Microsoft.IdentityModel.Tokens;
using NET8_Microservices_AuthAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NET8_Microservices_AuthAPI.Services.IServices
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions jwtOptions;

        public JwtTokenGenerator(JwtOptions jwtOptions)
        {
            //we have already configured JWTOptions in program.cs
            this.jwtOptions = jwtOptions;
        }
        public string GenerateToken(ApplicationUser applicationUser)
        {
            //extract secret key and encode it
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            //add claim list. it is a key value pair of email, username etc. Claims are stored inside token
            var claimList = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id)
            };

            //add tokenDescriptor which contains configuration properties for a token
            var tokenDescriptor = new SecurityTokenDescriptor {
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.Now.AddDays(7),
                //HmacSha256Signature is the default standard
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            //create a tokenHandler. This is used to create token
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
