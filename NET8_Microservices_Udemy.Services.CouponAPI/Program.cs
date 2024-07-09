using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NET8_Microservices_Udemy.Services.CouponAPI.Data;
using NET8_Microservices_Udemy.Services.CouponAPI.Mappings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // pass connection string from appsettings.json
});

//Register mappingconfig class in our service.
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.UseAuthorization();
//app.UseCors(config => config.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();
//Add to pipeline. when ever the application is restarted, it will check and apply migration
ApplyMigration();

app.Run();

void ApplyMigration()
{
    // get the AppDbContext service and check if any pending migrations are there. If yes, then update
    using(var scope = app.Services.CreateScope())
    {
        var db=scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (db.Database.GetPendingMigrations().Count() > 0)
        {
            db.Database.Migrate();
        }
    }
}