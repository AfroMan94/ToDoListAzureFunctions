using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoList.Core.Domain.Entities;

namespace ToDoList.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetItemsAsync(string query, Dictionary<string, object> queryParams);

        /// <summary>
        ///     Get one item by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetItemAsync(string id);
        Task AddItemAsync(T item);
        Task UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);

    }
}
