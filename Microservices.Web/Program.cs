using Microservices.Web.Interfaces;
using Microservices.Web.Models;
using Microservices.Web.Services;
using Microservices.Web.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//add httpcontectaccessor for working with cookies
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddSingleton<IBaseService,BaseService>();
builder.Services.AddSingleton<ICouponService, CouponService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IProductService, ProductService>();

/*
 *****Different method of authentication middleware configuration*****
AddJwtBearer: For JWT Bearer token authentication, mostly used in API scenarios.
AddCookie: For cookie-based authentication, mostly used in web applications.
AddOAuth: For OAuth 2.0 authentication, often used with third-party providers.
AddBearerToken: Not a standard method, potentially refers to custom bearer token authentication similar to AddJwtBearer
*/

//Register Cookie Authentication 
//In web app we use cookie-based authentication.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.LoginPath = "/AuthAPI/Login"; // [Authorize] attribute is used in HomeController/Details method.
                                              // So if an unauthorized user tries to view detials, it is redirected to Login page
        options.AccessDeniedPath = "/AuthAPI/AccessDenied";//need to add this action method
    });


//Add HttpClient for various services.

builder.Services.AddHttpClient("CouponClient", client =>
{
    
    Common.RequestUri=builder.Configuration["Services:CouponService"];
});

builder.Services.AddHttpClient("AuthClient", config => {
    Common.RequestUri = builder.Configuration["Services:AuthService"];

});
builder.Services.AddHttpClient("ProductClient", config =>
{
    Common.RequestUri = builder.Configuration["Services:ProductService"];
});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
