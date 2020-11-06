using System.Collections.Generic;
using System.Threading.Tasks;
using Radeoh.DAL.Models;

namespace Radeoh.DAL.Repository
{
    public class FavoriteRepository
    {
        public Task<List<Favorite>> GetAsync()
        {
            return RadeohDatabase.Database.Table<Favorite>().ToListAsync();
        }

        public Task<Favorite> FindById(int id)
        {
            return RadeohDatabase.Database.Table<Favorite>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(Favorite item)
        {
            return item.Id != 0
                ? RadeohDatabase.Database.UpdateAsync(item)
                : RadeohDatabase.Database.InsertAsync(item);
        }

        public Task<int> DeleteAsync(Favorite item)
        {
            return RadeohDatabase.Database.DeleteAsync(item);
        }
    }
}