using DapperConnection;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Providers;
using Services.Interfaces;
using static Services.Helpers.Delegates;
using Microsoft.Extensions.Logging;
using Services.Helpers;

namespace ORMComparison
{

    public static class HostConfig
    {
        public static IHostBuilder Configure(this IHostBuilder builder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<DataContext>(options => {
                    options.UseSqlServer(configuration.GetConnectionString("EntityFramework"));
                    });
                services.Configure<DatabaseSettings>(settings => settings.ConnectionString = configuration.GetConnectionString("Dapper") ?? "");
                services.AddOptions();

                services.AddSingleton<IQueryStringService, QueryStringService>();
                services.AddSingleton<IMappingService, MappingService>();
                services.AddSingleton<IResultService, ResultService>();
                services.AddScoped<DapperDataService>();
                services.AddScoped<EFDataService>();
                services.AddSingleton<DataServiceResolver>(provider => 
                    ormType => 
                    ormType switch
                {
                    ORMType.Dapper => provider.GetRequiredService<DapperDataService>(),
                    ORMType.EntityFrameWork => provider.GetRequiredService<EFDataService>(),
                    _ => throw new KeyNotFoundException(ormType.ToString())
                });
                services.AddSingleton<IExecutor, Executor>();
            })
                .ConfigureLogging(logging => logging.ClearProviders());

            return builder;
        }
    }
}
