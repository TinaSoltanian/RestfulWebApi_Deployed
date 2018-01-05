using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Services;

namespace WebApp.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomeMiddleware>();
        }

        public static async void AddSeedData(this IApplicationBuilder app)
        {
            var seedDataService = app.ApplicationServices.GetRequiredService<ISeedDataService>();
            await seedDataService.EnsureSeedData();
        }
    }
}
