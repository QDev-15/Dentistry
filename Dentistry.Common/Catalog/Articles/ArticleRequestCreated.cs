
using Microsoft.AspNetCore.Http;

namespace Dentistry.ViewModels.Catalog.Articles
{
    public class ArticleRequestCreated
    {
        public string Alias { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 0;
        public bool showHome { get; set; } = false;
        public List<IFormFile> ThumbnailImages { get; set; } = new List<IFormFile>();
    }
}
