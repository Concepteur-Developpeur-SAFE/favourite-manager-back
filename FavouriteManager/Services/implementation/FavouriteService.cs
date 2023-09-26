using FavouriteManager.Data;
using System.Collections.Generic;
using System.Linq;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services.implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDBContext _appDbContext;
        public FavouriteService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public long Test(int id)
        {
            return _appDbContext.Test(id);
        }
        public List<Favourite> FilterByCategory(long id)
        {
            // Using LINQ to filter favorites list by Category
            return _appDbContext.favourites.Where(fav => fav.Category.Id == id).ToList();
        }
        public List<Favourite> SortByCategory()
        {
            // Using LINQ to sort list by Category
            return _appDbContext.favourites.OrderBy(fav => fav.Category.Label).ToList();            
        }
        public List<Favourite> SortByDate()
        {
            // Using LINQ to sort list by Date
            return _appDbContext.favourites.OrderBy(fav => fav.UpdatedAt).ToList();
        }
    }
}
