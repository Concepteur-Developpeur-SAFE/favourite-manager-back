using System.Net;
using FavouriteManager.DTO;
using FavouriteManager.Data;
using FavouriteManager.Exception;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services.implementation
{
    /// <summary>
    /// Service responsible for managing favorites.
    /// </summary>
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDBContext _appDbContext;

        /// <summary>
        /// Initializes a new instance of the FavoriteService service.
        /// </summary>
        /// <param name="appDbContext">The database context of the application.</param>
        public FavouriteService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Creates a new favorite.
        /// </summary>
        /// <param name="favourite">The Favorite object to create.</param>
        /// <returns>The favorite created with its unique id</returns>
        public FavouriteResponse Create(CreateFavouriteRequest favourite)
        {
            Category category = _appDbContext.categories.Where(cat => cat.Id == favourite.CategoryId).FirstOrDefault();
  
            Favourite newFavourite = new Favourite
            {
                Link = favourite.Link,
                Label = favourite.Label,
                Category = category,
                UpdatedAt = DateTime.Now
            };
            //Check if the category was found
            if (category == null)
            {
                throw new NotFoundException();
            }

            //Check if the added favorite already exists based on a criterion (Link)
            bool exists = _appDbContext.favourites.Any(fav => fav.Link == newFavourite.Link);

            if (exists)
            {
                //Throw an exception if the favorite already exists
                throw new FavoriteAlreadyExistsException();
            }

            if (string.IsNullOrWhiteSpace(newFavourite.Link) || string.IsNullOrWhiteSpace(newFavourite.Label) || newFavourite.Category == null)
            {
                throw new ValidationException();
            }

            //Check if the added favorite link is valid
            CheckLinksAsync(newFavourite);

            //If the favorite does not yet exist, add it to the list of existing favorites          
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

        /// <summary>
        /// Get the list of all favorites.
        /// </summary>
        /// <returns>A list of all favorites present in the database.</returns>
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

        /// <summary>
        /// Updates the information of an existing favorite in the database.
        /// </summary>
        /// <param name="favourite">The Favorite object to update with the new information.</param>
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

        /// <summary>
        /// Delete a list of favorite from the database based on its unique ids.
        /// </summary>
        /// <param name="ids">The list of unique ids of the favorites to delete.</param>
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

        /// <summary>
        /// Filter the favorites list based on the specified category.
        /// </summary>
        /// <param name="id">The category id used for filtering.</param>
        /// <returns>A list of favorites belonging to the specified category.</returns>
        public List<FavouriteResponse> FilterByCategory(long id)
        {
            // Using LINQ to filter favorites list by Category
            return Get().Where(fav => fav.Category.Id == id).ToList();
         
        }

        /// <summary>
        /// Sort the favorites list by category.
        /// </summary>
        /// <returns>A list of favorites sorted (in ascending order) by category.</returns>
        public List<FavouriteResponse> SortByCategory()
        {
            // Using LINQ to sort list by Category
            return Get().OrderBy(fav => fav.Category.Label).ToList();            
        }

        /// <summary>
        /// Sort the favorites list by category.
        /// </summary>
        /// <returns>A list of favorites sorted (in descending order) by category.</returns>
        public List<FavouriteResponse> SortByCategoryDesc()
        {
            // Using LINQ to sort list by Category
            return Get().OrderByDescending(fav => fav.Category.Label).ToList();
        }

        /// <summary>
        /// Sort the favorites list by the last update.
        /// </summary>
        /// <returns>A list of favorites sorted (in ascending order) by the last update.</returns>
        public List<FavouriteResponse> SortByDate()
        {
            // Using LINQ to sort list by Date
            return Get().OrderBy(fav => fav.UpdatedAt).ToList();
        }

        /// <summary>
        /// Sort the favorites list by the last update.
        /// </summary>
        /// <returns>A list of favorites sorted (in descending order) by the last update.</returns>
        public List<FavouriteResponse> SortByDateDesc()
        {
            // Using LINQ to sort list by Date
            return Get().OrderByDescending(fav => fav.UpdatedAt).ToList();
        }

        /// <summary>
        /// Checks the validity of links in a favorite asynchronously.
        /// </summary>
        /// <param name="fav">The Favorite object containing the links to check.</param>
        /// <returns>A task representing the link verification operation.</returns>
        public async Task CheckLinksAsync(Favourite fav)
        {
            var httpClientHandler = new HttpClientHandler();
            var httpClient = new HttpClient(httpClientHandler);
            var response = await httpClient.GetAsync(fav.Link);
            List<HttpStatusCode> allowedCodes = new List<HttpStatusCode>()
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.Forbidden
            };

            if (response.IsSuccessStatusCode || allowedCodes.Contains(response.StatusCode))
            {
                fav.IsValid = true;
            } else
            {
                fav.IsValid = false;
            }           
        }
    }
}
