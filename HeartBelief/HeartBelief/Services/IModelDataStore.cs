using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeartBelief.Services
{
    public interface IModelDataStore<T>
    {
        Task<T> GetItemAsync(int id);
        Task<List<T>> GetItemsAsync();
        Task<int> SaveItemAsync(T item);
        Task<int> DeleteItemAsync(T item);
    }
}
