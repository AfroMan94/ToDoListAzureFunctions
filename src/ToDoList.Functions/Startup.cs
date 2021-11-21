using System.IO;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.AppSettings;
using ToDoList.Infrastructure.CosmosDbData.Repository;
using ToDoList.Infrastructure.Extensions;

[assembly: FunctionsStartup(typeof(ToDoList.Functions.Startup))]

namespace ToDoList.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Use a singleton Configuration throughout the application
            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            services.AddMediatR(typeof(Startup));

            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ToDoListCosmosDb").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                cosmosDbConfig.PrimaryKey,
                cosmosDbConfig.DatabaseName,
                cosmosDbConfig.Containers);

            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
        }
    }
}