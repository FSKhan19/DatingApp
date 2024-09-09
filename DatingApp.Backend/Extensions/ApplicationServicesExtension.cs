using DatingApp.Backend.Data;
using DatingApp.Backend.Services.Interfaces;
using DatingApp.Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();

            // Add services to the container.
            services.AddDbContext<DatingAppContext>(options =>
            {
                // Retrieve the connection string from user secrets
                var connectionString = configuration.GetConnectionString("DatingAppContext");
                options.UseSqlite(connectionString);
            });

            return services;
        }
    }
}
