using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services
{
    public interface IFavouriteService
    {
        FavouriteResponse Create(CreateFavouriteRequest favourite);

        List<FavouriteResponse> Get();

        void Update(UpdateFavouriteRequest favourite);

        void Delete(List<long> ids);

        List<Favourite> FilterByCategory(long id);

        List<Favourite> SortByCategory();

        List<Favourite> SortByDate();
    }

}
