using FavouriteManager.DTO;

namespace FavouriteManager.Services
{
    /// <summary>
    /// Interface defining service operations for managing categories.
    /// </summary>
    public interface ICategoryService
    {
        CategoryResponse Create(CreateCategoryRequest category);

        List<CategoryResponse> Get();

        void Update(UpdateCategoryRequest category);

        void Delete(List<long> ids);
    }
}
