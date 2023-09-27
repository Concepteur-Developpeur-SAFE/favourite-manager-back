using FavouriteManager.DTO;

namespace FavouriteManager.Services
{
    public interface ICategoryService
    {
        CategoryResponse Create(CreateCategoryRequest category);

        List<CategoryResponse> Get();

        void Update(UpdateCategoryRequest category);

        void Delete(List<long> ids);
    }
}
