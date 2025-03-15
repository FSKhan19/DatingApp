using DatingApp.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.Backend.Extensions
{
    public static class SeedDataExtensions
    {
        public static async Task ApplySeedDataAsync(this IHost host)
        {
            // Check if seeding is enabled in app configuration
            var config = host.Services.GetRequiredService<IConfiguration>();
            var isSeedEnabled = config.GetValue<bool>("Database:IsSeedingEnabled");

            if (!isSeedEnabled)
                return; // Exit early if seeding is not enabled

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<DatingAppContext>();

                // Apply migrations
                await context.Database.MigrateAsync();

                // Seed data
                await Seed.SeedUsers(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred during migration or seeding.");
            }
        }
    }
}
