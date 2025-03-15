using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

namespace DatingApp.Backend.Extensions
{
    public static class FluentValidationExtension
    {
        /// <summary>
        /// Configures FluentValidation for automatic validation and registers validators from the specified assembly.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assembly">The assembly to scan for validators.</param>
        /// <param name="lifetime">The lifetime for registered validators (default is Scoped).</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddFluentValidationWithAutoRegistration(
            this IServiceCollection services,
            Assembly assembly,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
        {
            // Enable automatic validation
            services.AddFluentValidationAutoValidation();

            // Scan the assembly for all types implementing IValidator<T>
            var validatorTypes = assembly.GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface &&
                               type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>)));

            // Register each validator with its corresponding interface
            foreach (var validatorType in validatorTypes)
            {
                var interfaceType = validatorType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>));

                services.Add(new ServiceDescriptor(interfaceType, validatorType, lifetime));
            }

            return services;
        }
    }
}
