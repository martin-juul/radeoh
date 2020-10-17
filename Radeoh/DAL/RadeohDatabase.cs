using System;
using System.Threading.Tasks;
using SQLite;
using System.Linq;
using Radeoh.Models;

namespace Radeoh.DAL
{
    public class RadeohDatabase
    {
        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer =
            new Lazy<SQLiteAsyncConnection>(
                () => new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags));

        private static SQLiteAsyncConnection Database => LazyInitializer.Value;
        public static bool Initialized = false;

        public RadeohDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!Initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Station).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Station)).ConfigureAwait(false);
                }

                Initialized = true;
            }
        }
    }
}