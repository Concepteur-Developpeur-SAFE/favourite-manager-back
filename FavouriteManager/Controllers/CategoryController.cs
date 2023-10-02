using FavouriteManager.DTO;
using FavouriteManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteManager.Controllers
{
    /// <summary>
    /// Controller to manage category.
    /// </summary>
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="category">New category information.</param>
        /// <returns>added category.</returns>
        [HttpPost("create")]
        public CategoryResponse Create(CreateCategoryRequest category)
        {
            return _categoryService.Create(category);
        }

        /// <summary>
        /// Get the list of all categories.
        /// </summary>
        /// <returns>The list of categories.</returns>
        [HttpGet("get")]
        public List<CategoryResponse> Get()
        {
            return _categoryService.Get();
        }

        /// <summary>
        /// Updates information for an existing category.
        /// </summary>
        /// <param name="category">The new category information.</param>
        [HttpPost("update")]
        public void Update(UpdateCategoryRequest category)
        {
            _categoryService.Update(category);
        }

        /// <summary>
        /// Delete a list of categories by their ids.
        /// </summary>
        /// <param name="ids">The categories ids to delete.</param>
        [HttpDelete("delete")]
        public void Delete(List<long> ids)
        {
            _categoryService.Delete(ids);
        }
    }
}
