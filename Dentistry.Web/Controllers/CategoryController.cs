using Dentisty.Data;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryReposiroty _categoryReposiroty;
        public CategoryController(ICategoryReposiroty categoryReposiroty)
        {
            _categoryReposiroty = categoryReposiroty;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("danh-muc/{alias}")]
        public async Task<IActionResult> Detail(string alias) {
            var category = await _categoryReposiroty.GetByAlias(alias);
            return View(category.ReturnViewModel());
        }
    }
}
