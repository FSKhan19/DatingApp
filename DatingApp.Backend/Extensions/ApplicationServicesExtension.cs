using DatingApp.Backend.Data;
using DatingApp.Backend.Services.Interfaces;
using DatingApp.Backend.Services;
using Microsoft.EntityFrameworkCore;
using DatingApp.Backend.Core.Repositories;
using DatingApp.Backend.Core;
using DatingApp.Backend.Data.Repositories;

namespace DatingApp.Backend.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Explicit registrations
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepositoryBase<>)); // Register closed generic first
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepositoryBase<,>)); // Registered as scopped bcz of DBContext
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auto-registration by convention
            RegisterServicesByInterface<IScopedService>(services, ServiceLifetime.Scoped);
            RegisterServicesByInterface<ITransientService>(services, ServiceLifetime.Transient);
            RegisterServicesByInterface<ISingletonService>(services, ServiceLifetime.Singleton);

            return services;
        }

        private static void RegisterServicesByInterface<TInterface>(
            IServiceCollection services,
            ServiceLifetime lifetime)
        {
            var interfaceType = typeof(TInterface);
            var assembly = interfaceType.Assembly;

            foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract))
            {
                if (!interfaceType.IsAssignableFrom(type)) continue;

                foreach (var implementedInterface in type.GetInterfaces()
                    .Where(i => i != interfaceType && !i.IsGenericTypeDefinition))
                {
                    var interfaceKey = implementedInterface.AssemblyQualifiedName;

                    // Check using assembly-qualified name
                    if (services.Any(sd =>
                        sd.ServiceType.AssemblyQualifiedName == interfaceKey)) continue;

                    var registration = ServiceDescriptor.Describe(
                        implementedInterface,
                        type,
                        lifetime
                    );

                    services.Add(registration);
                }
            }
        }

    }
}
