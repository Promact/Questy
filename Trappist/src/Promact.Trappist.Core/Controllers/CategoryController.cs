using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.Repository.Categories;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/Questions")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _categoryRepository.GetAllCategories();
            return Ok(categoryList);
        }
    }

}
