using Dentistry.ViewModels.Catalog.Articles;
using Dentisty.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml.Linq;

namespace Dentistry.Web.Controllers
{
    public class SitemapController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        public SitemapController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        [HttpGet("sitemap.xml")]
        public async Task<IActionResult> Sitemap()
        {
            List<ArticleVm> articles = await _articleRepository.SiteMap();

            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var urlset = new XElement(ns + "urlset");

            foreach (var article in articles)
            {
                var urlElement = new XElement(ns + "url",
                    new XElement(ns + "loc", $"{Request.Scheme}://{Request.Host}/bai-viet/{article.Alias}"),
                    new XElement(ns + "lastmod", article.UpdatedDate.ToString("yyyy-MM-dd"))
                );
                urlset.Add(urlElement);
            }

            

            var sitemap = new XDocument(urlset);
            return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
        }
    }
}
