using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Admin.Controllers
{
    public class ArticlesController : BaseController
    {
        private readonly IArticleRepository
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var articles = a
        }
    }
}
