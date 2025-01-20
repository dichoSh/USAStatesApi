using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsriStatesApi.Tests
{
    public class TestsStartup(IConfiguration configuration) : Startup(configuration)
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<EsriDbContext>(options =>
             {
                 options.UseInMemoryDatabase("Esri");
             });

            AddRestClients(services);

            ConfigureOptions(configuration, services);

            AddServices(services);

        }
    }
}
