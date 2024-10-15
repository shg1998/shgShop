using Carter;

namespace Ordering.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection service)
        {
            service.AddCarter();
            return service;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();
            return app;
        }
    }
}
