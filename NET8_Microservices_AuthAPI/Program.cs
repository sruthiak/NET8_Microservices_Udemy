using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NET8_Microservices_AuthAPI.Data;
using NET8_Microservices_AuthAPI.Models;
using NET8_Microservices_AuthAPI.Services.IServices;

var builder = WebApplication.CreateBuilder(args);
//Add DbContext
builder.Services.AddDbContext<AppDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")
));

//Add IdentityDbContext
builder.Services.AddIdentity<ApplicationUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Bind JwtOptions from appsettings. It is used in JwrTokenGenerator.cs for creating token.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("APISettings:JwtOptions"));

//Register JwtOption to be available for injection
builder.Services.AddScoped(resolver => resolver.GetRequiredService<IOptions<JwtOptions>>().Value);

//Register IAuth Service, IJwtTokenGenerator to be available for injection
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Add UseAuthentication middleware. Authentication comes before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
AddMigration();
app.Run();

void AddMigration(){
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}
