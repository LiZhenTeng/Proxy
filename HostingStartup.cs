using Proxy.IServices;
using Proxy.Services;

[assembly: HostingStartup(typeof(Proxy.HostingStartup))]
namespace Proxy
{
    public class HostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);
        }
        private void ConfigureServices(WebHostBuilderContext context, IServiceCollection services)
        {     
            services.AddScoped<IFetch,Fetch>();
            services.AddScoped<ICache,Cache>();
            services.AddHttpClient();
        }
    }
}