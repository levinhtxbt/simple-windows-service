using Microsoft.Extensions.DependencyInjection;

namespace WindowsServiceSample.Domain
{
    public interface IStartup
    {
        IServiceCollection ConfigureServices(IServiceCollection services);
    }
}