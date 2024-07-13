using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NET8_Microservices_Udemy.Services.CouponAPI.Extensions
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
    public static class WebApplicationBuilderExtensions
    {
        //static and this keyword is necessary when creating extension methods
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {

            var secret = builder.Configuration.GetValue<string>("APISettings:Secret");
            var issuer = builder.Configuration.GetValue<string>("APISettings:Issuer");
            var audience = builder.Configuration.GetValue<string>("APISettings:Audience");

            var key = Encoding.ASCII.GetBytes(secret);

            /*
             *****Different method of authentication middleware configuration*****
            AddJwtBearer: For JWT Bearer token authentication, mostly used in API scenarios.
            AddCookie: For cookie-based authentication, mostly used in web applications.
            AddOAuth: For OAuth 2.0 authentication, often used with third-party providers.
            AddBearerToken: Not a standard method, potentially refers to custom bearer token authentication similar to AddJwtBearer
            */

            //In API we use Jwt authentication. With this we can use [Authorize] attribute
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                //what all parameters we have to validate
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(key)


                };
            });

            return builder;

        }
    }
}
