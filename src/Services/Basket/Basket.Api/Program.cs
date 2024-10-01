using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(typeof(Program).Assembly);
    options.AddOpenBehavior(typeof(ValidationBehavior<,>));
    options.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCard>().Identity(s => s.Username);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHealthChecks("/health", new HealthCheckOptions
{
    // this make response more nice and clean with new structure!!
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
