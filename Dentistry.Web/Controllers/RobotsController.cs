using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Dentistry.Web.Controllers
{
    public class RobotsController : Controller
    {
        [HttpGet("robots.txt")]
        public IActionResult Robots()
        {
            var sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine("Allow: /");
            sb.AppendLine("Sitemap: " + $"{Request.Scheme}://{Request.Host}/sitemap.xml");

            return Content(sb.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}
