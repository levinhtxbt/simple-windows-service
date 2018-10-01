using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using WindowsServiceSample.Domain;
using Microsoft.Extensions.Configuration;

namespace WindowsServiceSample.Extensions
{
    public static class ServiceBaseLifetimeHostExtensions
    {
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                .ConfigureServices((hostContext, services) =>
                    services.AddSingleton<IHostLifetime, ServiceBaseLifetime>());
        }

        public static Task RunAsServiceAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
        {
            return hostBuilder.UseServiceBaseLifetime().Build().RunAsync(cancellationToken);
        }

        public static IHostBuilder UseAppsetting(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(((hostContext, configurationBuilder) =>
            {
                var env = hostContext.HostingEnvironment;

                configurationBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                configurationBuilder.AddEnvironmentVariables();

                var configuration = configurationBuilder.Build();
            }));
        }

        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }

        public static IHostBuilder UseStartup(this IHostBuilder hostBuilder, Type startupType)
        {
            return hostBuilder.ConfigureServices((Action<IServiceCollection>)(services =>
            {
                if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                {
                    var startupInstance = Activator.CreateInstance(startupType) as IStartup;
                    services = startupInstance.ConfigureServices(services);
                }
            }));

        }
    }
}