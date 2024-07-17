using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NET8_Microservices_ShoppingCartAPI.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
       
        public static WebApplicationBuilder AddAppAuthentication(this WebApplicationBuilder builder)
        {
            var secret = builder.Configuration.GetValue<string>("APISettings:Secret");
            var audience = builder.Configuration.GetValue<string>("APISettings:Audience");
            var issuer = builder.Configuration.GetValue<string>("APISettings:Issuer");
            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience=audience,
                    ValidIssuer=issuer,
                    IssuerSigningKey=new SymmetricSecurityKey(key)

                };
            });
            return builder;
        }
    }
}
