using FavouriteManager.Data;
using FavouriteManager.Persistence.entity;
using FavouriteManager.DTO;

namespace FavouriteManager.Services.implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDBContext _appDbContext;

        public FavouriteService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public FavouriteResponse Create(CreateFavouriteRequest favourite)
        {
            Favourite newFavourite = new Favourite {
                Link = favourite.Link,
                Label = favourite.Label,
                Category = _appDbContext.categories.Where(cat => cat.Id == favourite.CategoryId).FirstOrDefault(),
                UpdatedAt = DateTime.Now
            };

            _appDbContext.favourites.Add(newFavourite);
            _appDbContext.SaveChanges();

            return new FavouriteResponse(
                newFavourite.Id,
                newFavourite.Label,
                newFavourite.Link,
                new CategoryResponse(newFavourite.Category.Id, newFavourite.Category.Label),
                newFavourite.UpdatedAt
                );
        }

        public List<FavouriteResponse> Get()
        {
            List<FavouriteResponse> favResponseList = _appDbContext.favourites.Select(
               fav => new FavouriteResponse(
                   fav.Id,
                   fav.Label,
                   fav.Link,
                   new CategoryResponse(fav.Category.Id, fav.Category.Label),
                   fav.UpdatedAt
                   )).ToList();
            return favResponseList;
        }

        public void Update(UpdateFavouriteRequest favourite)
        {
            _appDbContext.favourites.Where(fav => fav.Id == favourite.Id).ToList().ForEach(
                fav =>
                {
                    fav.Label = favourite.Label;
                    fav.Link = favourite.Link;
                    fav.Category = _appDbContext.categories.Where(cat => cat.Id == favourite.CategoryId).FirstOrDefault();
                    fav.UpdatedAt = DateTime.Now;
                }
             );
            _appDbContext.SaveChanges();
        }

        public void Delete(List<long> ids)
        {
            foreach (var id in ids)
            {
                var favToDelete = _appDbContext.favourites.FirstOrDefault(f => f.Id == id);
                if (favToDelete != null)
                {
                    _appDbContext.favourites.Remove(favToDelete);
                }
                _appDbContext.SaveChanges();
            }
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
