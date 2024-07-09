using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.IdentityModel.Tokens;
using NET8_Microservices_AuthAPI.Models;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
        /// <summary>
        /// JWT contains the structure "header.payload.signature"
        /// check jwt.io website
        /// ClaimsIdentity is the collection of claims. It is serailized and stored in Jwt token payload.
        /// This allows the server to authenticate requests without relying on server-side sessions, 
        /// making it suitable for microservices architectures and APIs.
        /// 
        /// How They Work Together
        ///Authentication Flow: When a user logs in, the server verifies their credentials and 
        ///creates a ClaimsIdentity representing the authenticated user.Depending on the 
        ///authentication mechanism:
        ///Cookie-Based: The server serializes the ClaimsIdentity into a cookie, which is sent to the client's
        ///browser. The cookie is then included in subsequent requests, allowing the server to identify and
        ///authenticate the user.
        ///JWT-Based: The server generates a JWT containing the user's claims and signs it using a 
        ///secret key. The JWT is sent to the client, who includes it in the Authorization header of 
        ///subsequent requests. The server verifies the JWT's signature and decodes the claims to 
        ///authenticate and authorize the user.
        ///Authorization: Both ClaimsIdentity and JWT carry claims about the user's identity, roles,
        ///and permissions. This information is used by the server to authorize access to resources or 
        ///operations based on the user's privileges.


        /// </summary>
        /// <param name="applicationUser"></param>
        /// <returns></returns>
        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles)
        {
            //extract secret key and encode it
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            //add claim list. it is a key value pair of email, username etc. Claims are stored inside token
            var claimList = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Name, applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id)
            };

            //add roles to claimList(ClaimsIdentity)
            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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
