using FavouriteManager.Data;
using FavouriteManager.Persistence.entity;
using FavouriteManager.DTO;
using FavouriteManager.Exception;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using System.Net;

namespace FavouriteManager.Services.implementation
{
    public class FavouriteService : IFavouriteService
    {
        private readonly AppDBContext _appDbContext;
        //private readonly HttpClient _httpClient;

        public FavouriteService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        /*public FavouriteService(AppDBContext appDbContext, HttpClient httpClient)
        {
            _appDbContext = appDbContext;
            _httpClient = httpClient;
        }*/

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

        public List<FavouriteResponse> FilterByCategory(long id)
        {
            // Using LINQ to filter favorites list by Category
            return Get().Where(fav => fav.Category.Id == id).ToList();
         
        }

        public List<FavouriteResponse> SortByCategory()
        {
            // Using LINQ to sort list by Category
            return Get().OrderBy(fav => fav.Category.Label).ToList();            
        }

        public List<FavouriteResponse> SortByCategoryDesc()
        {
            // Using LINQ to sort list by Category
            return Get().OrderByDescending(fav => fav.Category.Label).ToList();
        }

        public List<FavouriteResponse> SortByDate()
        {
            // Using LINQ to sort list by Date
            return Get().OrderBy(fav => fav.UpdatedAt).ToList();
        }
        public List<FavouriteResponse> SortByDateDesc()
        {
            // Using LINQ to sort list by Date
            return Get().OrderByDescending(fav => fav.UpdatedAt).ToList();
        }

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
