using Mapster;
using MapsterMapper;
using System.Reflection;

namespace DatingApp.Backend.Extensions
{
    public static class MapsterExtension
    {
        public static void AddMapsterConfig(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Scan the assembly and register all IRegister implementations
            config.Scan(Assembly.GetExecutingAssembly());

            // Register Mapster with DI
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>(); // Use Mapster’s DI
        }
    }

}
