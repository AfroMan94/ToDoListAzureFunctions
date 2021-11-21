using ToDoList.Core.Interfaces;
using ToDoList.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Storage.Net;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos;
using ToDoList.Infrastructure.AppSettings;
using ToDoList.Infrastructure.CosmosDbData;
using ToDoList.Infrastructure.CosmosDbData.Interfaces;

namespace ToDoList.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Register a singleton instance of Cosmos Db Container Factory, which is a wrapper for the CosmosClient.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="endpointUrl"></param>
        /// <param name="primaryKey"></param>
        /// <param name="databaseName"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerInfo> containers)
        {
            var client = new CosmosClient(endpointUrl, primaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            cosmosDbClientFactory.EnsureDbSetupAsync().Wait();

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }

        /// <summary>
        ///     Setup Azure Blob storage
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupStorage(this IServiceCollection services, IConfiguration configuration)
        {
            StorageFactory.Modules.UseAzureBlobStorage();

            // Register IBlobStorage, which is used in AzureBlobStorageService
            // Avoid using IBlobStorage directly outside of AzureBlobStorageService.
            services.AddScoped(
                factory => StorageFactory.Blobs.FromConnectionString(configuration.GetConnectionString("StorageConnectionString")));

            services.AddScoped<IStorageService, AzureBlobStorageService>();
        }

    }
}
