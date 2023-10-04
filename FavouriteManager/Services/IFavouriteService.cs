using FavouriteManager.DTO;

namespace FavouriteManager.Services
{
    /// <summary>
    /// Interface defining service operations for managing favorites.
    /// </summary>
    public interface IFavouriteService
    {
        FavouriteResponse Create(CreateFavouriteRequest favourite);

        List<FavouriteResponse> Get();

        void Update(UpdateFavouriteRequest favourite);

        void Delete(List<long> ids);

        List<FavouriteResponse> FilterByCategory(long id);

        List<FavouriteResponse> SortByCategory();
        List<FavouriteResponse> SortByCategoryDesc();

        List<FavouriteResponse> SortByDate();
        List<FavouriteResponse> SortByDateDesc();
    }
}
