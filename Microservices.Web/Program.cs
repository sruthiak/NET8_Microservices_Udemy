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

//Register Cookie Authentication 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccessDenied";//need to add this action method
    });


//Add HttpClient for various services.

builder.Services.AddHttpClient("CouponClient", client =>
{
    
    Common.RequestUri=builder.Configuration["Services:CouponService"];
});

builder.Services.AddHttpClient("AuthClient", config => {
    Common.RequestUri = builder.Configuration["Services:AuthService"];

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
