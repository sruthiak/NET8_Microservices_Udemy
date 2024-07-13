using Microsoft.EntityFrameworkCore;
using NET8_Microservices_ProductAPI.Data;
using NET8_Microservices_ProductAPI.Extensions;
using NET8_Microservices_ProductAPI.Mappings;

var builder = WebApplication.CreateBuilder(args);

//registers the AppDbContext with the dependency injection container and configures
//it to use SQL Server with the provided connection string.

/*
 **** You can chain multiple configurations together:****
 services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("YourConnectionStringHere");
    options.UseLazyLoadingProxies();
    options.EnableSensitiveDataLogging();
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});
*/
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

//Register IMapper
var mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

//Register automapper 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//This is the extension method for Adding Authentication. Check Extension folder
//Configures the authentication services and sets
//up authentication schemes in the DI container within ConfigureServices.
builder.AddAppAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adds the authentication middleware to the request processing pipeline within Configure.
// It ensures that the authentication logic is applied to incoming requests. Validates thw JWT token
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}