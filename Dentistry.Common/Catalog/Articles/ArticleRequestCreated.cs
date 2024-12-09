
using Microsoft.AspNetCore.Http;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleRequestCreated
    {
        public string Alias { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CreatedById { get; set; }
        public List<IFormFile> ThumbnailImages { get; set; } = new List<IFormFile>();
    }
}
