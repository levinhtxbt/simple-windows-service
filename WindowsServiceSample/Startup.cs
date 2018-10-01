using Microsoft.Extensions.DependencyInjection;
using WindowsServiceSample.Domain;
using WindowsServiceSample.Services;

namespace WindowsServiceSample
{
    public class Startup : IStartup
    {
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<FileWriterService>();
            services.AddScoped<ITestService, TestService>();

            return services;
        }
    }
}