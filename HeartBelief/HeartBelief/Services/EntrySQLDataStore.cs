using HeartBelief.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeartBelief.Services
{
    public class EntrySQLDataStore : SQLDataStore<Entry>, IModelDataStore<Entry>
    {
        public Task<Entry> GetItemAsync(int id)
        {
            return Database.Table<Entry>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<List<Entry>> GetItemsAsync()
        {
            return Database.Table<Entry>().ToListAsync();
        }

        public Task<int> SaveItemAsync(Entry item)
        {
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Entry item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
