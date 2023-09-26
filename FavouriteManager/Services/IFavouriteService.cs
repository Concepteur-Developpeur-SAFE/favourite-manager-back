using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services
{
    public interface IFavouriteService
    {
        long Test(int id);
        List<Favourite> FilterByCategory(long id);
        FavouriteResponse Create(CreateFavouriteRequest favourite);
        List<FavouriteResponse> Get();

        void Update(UpdateFavouriteRequest favourite);
        void Delete(List<long> ids);
        List<Favourite> SortByCategory();
        List<Favourite> SortByDate();
    }

}
