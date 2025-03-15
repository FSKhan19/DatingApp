using DatingApp.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext - scoped lifetime
            services.AddDbContext<DatingAppContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DatingAppContext");
                options.UseSqlite(connectionString);
            });
        }
    }
}
