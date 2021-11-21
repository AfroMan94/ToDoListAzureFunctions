using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Infrastructure.CosmosDbData.Interfaces;
using ToDoList.Core.Domain.Entities;
using ToDoList.Core.Interfaces;

namespace ToDoList.Infrastructure.CosmosDbData.Repository
{
    public class ToDoItemRepository : CosmosDbRepository<ToDoItemEntity>, IToDoItemRepository
    {
        /// <summary>
        ///     CosmosDB container name
        /// </summary>
        public override string ContainerName { get; } = "ToDoItems";

        /// <summary>
        ///     Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public ToDoItemRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public async Task<IEnumerable<ToDoItemEntity>> GetItemsAsyncByDescription(string description)
        {
            string query = @"SELECT c.* FROM c WHERE c.description = @description";
            var queryParams = new Dictionary<string, object> {{"@description", description}};

            return await GetItemsAsync(query, queryParams);
        }
    }
}
