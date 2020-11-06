using System;
using System.Threading.Tasks;
using SQLite;
using System.Linq;
using Radeoh.DAL.Models;
using Radeoh.Models;

namespace Radeoh.DAL
{
    public class RadeohDatabase
    {
        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer =
            new Lazy<SQLiteAsyncConnection>(
                () => new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags));

        public static SQLiteAsyncConnection Database => LazyInitializer.Value;
        public static bool Initialized;

        public RadeohDatabase()
        {
           // InitializeAsync().SafeFireAndForget(false);
        }

        public async Task InitializeAsync()
        {
            if (!Initialized)
            {
                if (Database.TableMappings.All(m => m.MappedType.Name != nameof(Favorite)))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Favorite)).ConfigureAwait(false);
                }

                Initialized = true;
            }
        }
    }
}