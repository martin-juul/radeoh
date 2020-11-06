using Radeoh.DAL.Repository;

namespace Radeoh.DAL
{
    public class DbContext
    {
        private static RadeohDatabase _db;

        public static RadeohDatabase Database => _db ??= new RadeohDatabase();

        public FavoriteRepository favoriteRepository;
        
        public DbContext()
        {
            favoriteRepository = new FavoriteRepository();
        }
    }
}