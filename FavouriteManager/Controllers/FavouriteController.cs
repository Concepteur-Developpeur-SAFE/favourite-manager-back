using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;
using FavouriteManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteManager.Controllers
{
    [ApiController]
    [Route("api/favorite")]
    public class FavouriteController : ControllerBase
    {
        private readonly IFavouriteService _favouriteService;
        public FavouriteController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }

        [HttpPost("create")]
        public FavouriteResponse Create(CreateFavouriteRequest favourite)
        {
            return _favouriteService.Create(favourite);
        }

        [HttpGet("get")]
        public List<FavouriteResponse> Get()
        {
            return _favouriteService.Get();
        }

        [HttpPost("update")]
        public void Update(UpdateFavouriteRequest favourite)
        {
            
            _favouriteService.Update(favourite);
        }

        [HttpDelete("delete")]
        public void Delete(List<long> ids)
        {
           _favouriteService.Delete(ids);

        }


        [HttpGet("filter/{category}")]
        public List<Favourite> FilterByCategory(long id)
        {
            return _favouriteService.FilterByCategory(id);
        }

        [HttpGet("sortByCat")]
        public List<Favourite> SortByCategory()
        {
            return _favouriteService.SortByCategory();
        }

        [HttpGet("sortByDate")]
        public List<Favourite> SortByDate()
        {
            return _favouriteService.SortByDate();
        }
    }
}