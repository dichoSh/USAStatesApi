using DataAccess;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.EntityFrameworkCore;
using RestClients;
using RestClients.Interfaces;
using RestClients.Options;
using Scalar.AspNetCore;
using Services;
using Services.Interfaces;

namespace EsriStatesApi
{
    public class Startup(IConfiguration configuration)
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddOpenApi("doc");

            services.AddDbContextPool<EsriDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            });

            AddHangfire(configuration, services);

            AddRestClients(services);

            ConfigureOptions(configuration, services);

            AddServices(services);

        }


        public virtual void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(op =>
                {
                    op.OpenApiRoutePattern = "/openapi/doc.json";
                    op.Servers = configuration.GetSection("ScalarServers").Get<string[]>()!
                    .Select(x => new ScalarServer(x))
                    .ToArray();
                });
            }

            app.UseAuthorization();

            app.MapControllers();

            MigrateDatabase(app);

            SetupHangfire(app);
        }

        protected void SetupHangfire(WebApplication app)
        {
            app.UseHangfireDashboard(options: new DashboardOptions { Authorization = [new HangfireAuthorization()] });

            if (configuration.GetSection("Hangfire:SyncCounties:Enabled").Get<bool>())
            {
                var syncCoutriesCron = configuration.GetSection("Hangfire:SyncCounties:Cron").Get<string>();
                RecurringJob.AddOrUpdate<ISyncService>("sync-counties", s => s.SyncStatesPopulation(CancellationToken.None), syncCoutriesCron);
            }
            else
            {
                RecurringJob.RemoveIfExists("sync-counties");
            }

        }

        protected static void AddHangfire(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHangfire(options =>
            {
                options.UseSQLiteStorage(configuration.GetConnectionString("Default"));
            });

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = 1;
            });
        }

        protected static void AddServices(IServiceCollection services)
        {
            services.AddSingleton(TimeProvider.System);
            services.AddTransient<IUSAStatesService, USAStatesService>();
            services.AddTransient<ISyncLogService, SyncLogService>();
            services.AddTransient<ISyncService, SyncService>();
        }

        protected static void AddRestClients(IServiceCollection services)
        {
            services.AddTransient<IUSACountiesClient, USACountiesClient>();
        }

        protected static void ConfigureOptions(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<USACountiesClientOptions>(configuration.GetSection(nameof(USACountiesClientOptions)));
        }

        protected static void MigrateDatabase(WebApplication app)
        {
            var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<EsriDbContext>();
            db.Database.EnsureCreated();
            db.Database.Migrate();
            scope.Dispose();
        }
    }
}
