using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;

namespace NET8_Microservices_ProductAPI.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        /// <summary>
        /// This is used to validate the JWT token.
        /// 1.The client sends an HTTP request to the server 
        /// with the JWT included in the Authorization header of the request- check BaseService.cs
        /// 2.The request first passes through the JWT middleware (AddJwtBearer). This middleware validates the token.
        /// 3.The JWT middleware validates the token by checking its signature, issuer, audience, 
        /// and expiration date. If the token is valid, the middleware extracts the claims
        /// and creates a ClaimsPrincipal object, which represents the authenticated user.
        /// 4.When the request reaches a controller decorated with the [Authorize] attribute,
        /// the authorization middleware checks the ClaimsPrincipal object to ensure
        /// the user meets the authorization requirements.
        /// 5. You can configure additional policies and role-based authorization 
        /// using the AddAuthorization method. example below
        /// services.AddAuthorization(options =>
        ///{
        /// options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        ///});
        ///[Authorize(Policy = "AdminOnly")]
        ///6.If the token is invalid or the user is not authorized, the request 
        ///will be denied with an appropriate HTTP status code, usually 401 Unauthorized or 403 Forbidden.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            //get secret, audience and issuer from appsettings.json
            var secret = builder.Configuration.GetValue<string>("APISettings:Secret");
            var audience = builder.Configuration.GetValue<string>("APISettings:Audience");
            var issuer = builder.Configuration.GetValue<string>("APISettings:Issuer");

            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)

                };
            });


            return builder;
        }
    }
}
