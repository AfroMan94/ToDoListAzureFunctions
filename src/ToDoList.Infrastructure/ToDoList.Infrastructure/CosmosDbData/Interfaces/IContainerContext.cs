using Microsoft.Azure.Cosmos;
using ToDoList.Core.Domain.Entities;

namespace ToDoList.Infrastructure.CosmosDbData.Interfaces
{
    /// <summary>
    ///  Defines the container level context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContainerContext<T> where T : BaseEntity
    {
        string ContainerName { get; }
        PartitionKey ResolvePartitionKey(string entityId);
    }
}
