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

        public Task<Favorite> FindBySlug(string slug)
        {
            return RadeohDatabase.Database.Table<Favorite>()
                .Where(x => x.Slug == slug)
                .FirstOrDefaultAsync();
        }

        public Task<int> SaveAsync(Favorite item)
        {
            var exist = FindBySlug(item.Slug);

            return exist.Id != 0 ? DeleteAsync(item) : RadeohDatabase.Database.InsertAsync(item);
        }

        public Task<int> DeleteAsync(Favorite item)
        {
            return RadeohDatabase.Database.DeleteAsync(item);
        }
    }
}