using Microsoft.AspNetCore.Mvc;

namespace Dentistry.Web.Controllers
{
    public class ArticleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("bai-viet/{alias}")]
        public IActionResult Articles(string alias)
        {
            return View();
        }     
        [HttpGet("tin-tuc/{alias}")]
        public IActionResult News(string alias)
        {
            return View();
        }    
        [HttpGet("phan-hoi/{alias}")]
        public IActionResult FeedBack(string alias)
        {
            return View();
        }    
        [HttpGet("san-pham/{alias}")]
        public IActionResult Product(string alias)
        {
            return View();
        }
    }
}
