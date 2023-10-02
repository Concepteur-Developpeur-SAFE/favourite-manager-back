using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteManager.Controllers
{
    /// <summary>
    /// Controller to manage favorite.
    /// </summary>
    [ApiController]
    [Route("api/favorite")]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        /// <summary>
        /// Create a new favorite.
        /// </summary>
        /// <param name="favourite">New favorite information.</param>
        /// <returns>added favorite.</returns>
        [HttpPost("create")]
        public FavouriteResponse Create(CreateFavouriteRequest favourite)
        {
            return _favouriteService.Create(favourite);
            
        }

        /// <summary>
        /// Get the list of all favorites.
        /// </summary>
        /// <returns>The list of favorites.</returns>
        [HttpGet("get")]
        public List<FavouriteResponse> Get()
        {
            return _favouriteService.Get();
        }

        /// <summary>
        /// Updates information for an existing favorite.
        /// </summary>
        /// <param name="favourite">The new favorite information.</param>
        [HttpPost("update")]
        public void Update(UpdateFavouriteRequest favourite)
        {
            
            _favouriteService.Update(favourite);
        }

        /// <summary>
        /// Delete a list of favorites by their ids.
        /// </summary>
        /// <param name="ids">The favorites ids to delete.</param>
        [HttpDelete("delete")]
        public void Delete(List<long> ids)
        {
           _favouriteService.Delete(ids);

        }

        /// <summary>
        /// Get the list of filtered favorites.
        /// </summary>
        /// <param name="id">The category id used for filtering.</param>
        /// <returns>The list of filtered favorite.</returns>
        [HttpGet("filter/{id}")]
        public List<FavouriteResponse> FilterByCategory(long id)
        {
            return _favouriteService.FilterByCategory(id);
        }

        /// <summary>
        /// Get the list of sorted favorite by category.
        /// </summary>
        /// <returns>The list of sorted favorites in ascending order.</returns>
        [HttpGet("sortByCat")]
        public List<FavouriteResponse> SortByCategory()
        {
            return _favouriteService.SortByCategory();
        }

        /// <summary>
        /// Get the list of sorted favorite by category.
        /// </summary>
        /// <returns>The list of sorted favorites in descending order.</returns>
        [HttpGet("sortByCatDesc")]
        public List<FavouriteResponse> SortByCategoryDesc()
        {
            return _favouriteService.SortByCategoryDesc();
        }

        /// <summary>
        /// Get the list of sorted favorite by latest update.
        /// </summary>
        /// <returns>The list of sorted favorites in ascending order.</returns>
        [HttpGet("sortByDate")]
        public List<FavouriteResponse> SortByDate()
        {
            return _favouriteService.SortByDate();
        }

        /// <summary>
        /// Get the list of sorted favorite by latest update.
        /// </summary>
        /// <returns>The list of sorted favorites in descending order.</returns>
        [HttpGet("sortByDateDesc")]
        public List<FavouriteResponse> SortByDateDesc()
        {
            return _favouriteService.SortByDateDesc();
        }
    }
}
