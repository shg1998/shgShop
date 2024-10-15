using BuildingBlocks.Exceptions.Handler;
using Carter;

namespace Ordering.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddCarter();
            service.AddExceptionHandler<CustomExceptionHandler>();
            service.AddHealthChecks()
                .AddSqlServer(configuration.GetConnectionString("Database")!);
            return service;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health");
            return app;
        }
    }
}
