
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Infrastructure.Data;
using Core.models;
using Microsoft.AspNetCore.Identity;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddDbContext<AppIdentityDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"))
    );


builder.Services.AddSingleton<ConnectionMultiplexer>(c =>
{
    var config = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(config);
}
);

builder.Services.AddScoped<IBasketRepo, BasketRepo>();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IGenericRepo<Product>, GenericRepo<Product>>();
//builder.Services.AddScoped<IGenericRepo<T>, GenericRepo<T>>();
//builder.Services.AddScoped<IGenericRepo<T>, GenericRepo<T>>();




builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200/").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                      });
});


var app = builder.Build();

var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();

try
{
    var context = app.Services.GetRequiredService<AppDbContext>();
    await StoreContextSeed.SeedAsync(context, loggerFactory);
}
catch (Exception ex)
{
    var logger = LoggerFactory.Create(
        builder => builder.AddConsole()

    );
    logger.CreateLogger<Program>().LogError(ex.Message);
    // logger.LogError(ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();
app.UseStaticFiles();

app.Run();
