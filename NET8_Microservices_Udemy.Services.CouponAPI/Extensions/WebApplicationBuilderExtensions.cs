using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NET8_Microservices_Udemy.Services.CouponAPI.Extensions
{
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
