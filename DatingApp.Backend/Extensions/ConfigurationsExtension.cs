using DatingApp.Backend.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DatingApp.Backend.Extensions
{
    public static class ConfigurationsExtension
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var markedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.GetCustomAttributes(typeof(ConfigurationAttribute), false).Any());

            foreach (var type in markedTypes)
            {
                var attribute = type.GetCustomAttributes(typeof(ConfigurationAttribute), false)
                    .FirstOrDefault() as ConfigurationAttribute;

                if (attribute != null)
                {
                    // Get the generic Configure<TOptions> method
                    var configureMethod = typeof(OptionsServiceCollectionExtensions)
                        .GetMethods()
                        .FirstOrDefault(m => m.Name == "Configure" && m.GetParameters().Length == 2)
                        ?.MakeGenericMethod(type);

                    if (configureMethod != null)
                    {
                        // Get the configuration section
                        var configurationSection = configuration.GetSection(attribute.SectionName);

                        // Bind the configuration section to an instance of TOptions
                        var optionsInstance = Activator.CreateInstance(type);
                        configurationSection.Bind(optionsInstance);

                        // Create an Action<TOptions> delegate to configure the options
                        var configureDelegate = (Action<object>)((opts) =>
                        {
                            var properties = type.GetProperties();
                            foreach (var property in properties)
                            {
                                var value = property.GetValue(optionsInstance);
                                property.SetValue(opts, value);
                            }
                        });

                        // Invoke Configure<TOptions> dynamically
                        configureMethod.Invoke(null, new object[] { services, configureDelegate });
                    }
                }
            }

            return services;
        }

    }
}
