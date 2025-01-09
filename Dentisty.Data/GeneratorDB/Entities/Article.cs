using Dentistry.ViewModels.Enums;
using Dentisty.Data.Common.Enums;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ArtisleType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; set; }
        public Guid CreatedById { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<HomeArticle> HomeArticles { get; set; } = new List<HomeArticle>();
        public List<Tags> Tags { get; set; } = new List<Tags>();

        public Category Category { get; set; }
        public AppUser CreatedBy { set; get; }

    }
}
