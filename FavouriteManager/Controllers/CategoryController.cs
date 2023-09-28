using FavouriteManager.DTO;
using FavouriteManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FavouriteManager.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("create")]
        public CategoryResponse Create(CreateCategoryRequest category)
        {
            return _categoryService.Create(category);
        }

        [HttpGet("get")]
        public List<CategoryResponse> Get()
        {
            return _categoryService.Get();
        }

        [HttpPost("update")]
        public void Update(UpdateCategoryRequest category)
        {
            _categoryService.Update(category);
        }

        [HttpDelete("delete")]
        public void Delete(List<long> ids)
        {
            _categoryService.Delete(ids);
        }
    }
}
