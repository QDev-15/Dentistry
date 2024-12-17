using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Doctor Doctor { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<Category> Categories { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();
        public List<HomeArticle> HomeArticles { get; set; } = new List<HomeArticle>();
        public List<Slide> Slides { get; set; } = new List<Slide>();
    }
}
