using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeartBelief.Helpers.Extensions;
using HeartBelief.Models;
using SQLite;

namespace HeartBelief.Services
{
    public abstract class SQLDataStore<T>
    {
        protected static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        
        public SQLDataStore()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(T).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
                    initialized = true;
                }
            }
        }
    }
}