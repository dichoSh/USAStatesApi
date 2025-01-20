using EsriStatesApi.Tests;
using Microsoft.AspNetCore.Builder;

namespace Tests
{
    public class TestsBase()
    {
        protected IServiceProvider serviceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var builder = WebApplication.CreateBuilder();
            var startup = new TestsStartup(builder.Configuration);
            startup.ConfigureServices(builder.Services);
            var app = builder.Build();
            serviceProvider = app.Services;
        }


    }
}
