using FavouriteManager.Data;
using FavouriteManager.DTO;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services.implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDBContext _appDbContext;

        public CategoryService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public CategoryResponse Create(CreateCategoryRequest category)
        {
            Category newCategory = new Category
            {
                Label = category.Label
            };

            _appDbContext.categories.Add(newCategory);
            _appDbContext.SaveChanges();

            return new CategoryResponse(
                newCategory.Id,
                newCategory.Label
                );
        }

        public List<CategoryResponse> Get()
        {
            List<CategoryResponse> catResponseList = _appDbContext.categories.Select(
              cat => new CategoryResponse(
                  cat.Id,
                  cat.Label
                  )).ToList();
            return catResponseList;
        }

        public void Update(UpdateCategoryRequest category)
        {
            _appDbContext.categories.Where(cat => cat.Id == category.Id).ToList().ForEach(
               cat =>
               {
                   cat.Label = category.Label;
               }
            );
            _appDbContext.SaveChanges();
        }

        public void Delete(List<long> ids)
        {
            foreach (var id in ids)
            {
                var catToDelete = _appDbContext.categories.FirstOrDefault(c => c.Id == id);
                if (catToDelete != null)
                {
                    _appDbContext.categories.Remove(catToDelete);
                }
                _appDbContext.SaveChanges();
            }
        }
    }
}
