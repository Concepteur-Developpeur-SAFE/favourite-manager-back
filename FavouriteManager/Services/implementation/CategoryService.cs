using FavouriteManager.Data;
using FavouriteManager.DTO;
using FavouriteManager.Exception;
using FavouriteManager.Persistence.entity;

namespace FavouriteManager.Services.implementation
{
    /// <summary>
    /// Service responsible for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly AppDBContext _appDbContext;

        /// <summary>
        /// Initializes a new instance of the CategoryService service.
        /// </summary>
        /// <param name="appDbContext">The database context.</param>
        public CategoryService(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The Favorite object to create.</param>
        /// <returns>The category created.</returns>
        public CategoryResponse Create(CreateCategoryRequest category)
        {
            Category newCategory = new Category
            {
                Label = category.Label
            };
            //Check if the added category already exists based on a criterion (Label)
            bool exists = _appDbContext.categories.Any(cat => cat.Label == newCategory.Label);

            if (exists)
            {
                //Throw an exception if the category already exists
                throw new FavoriteAlreadyExistsException();
            }

            if (string.IsNullOrWhiteSpace(newCategory.Label))
            {
                throw new ValidationException();
            }

            _appDbContext.categories.Add(newCategory);
            _appDbContext.SaveChanges();

            return new CategoryResponse(
                newCategory.Id,
                newCategory.Label
                );
        }

        /// <summary>
        /// Gets the list of categories.
        /// </summary>
        /// <returns>A list of all categories present in the database.</returns>
        public List<CategoryResponse> Get()
        {
            List<CategoryResponse> catResponseList = _appDbContext.categories.Select(
              cat => new CategoryResponse(
                  cat.Id,
                  cat.Label
                  )).ToList();
            return catResponseList;
        }

        /// <summary>
        /// Updates the information of an existing category in the database.
        /// </summary>
        /// <param name="category">The category object to update with the new information.</param>
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

        /// <summary>
        /// Delete a list of categories from the database based on its unique ids.
        /// </summary>
        /// <param name="ids">The list of unique ids of the categories to delete.</param>
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
