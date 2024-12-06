using Dentisty.Data.Common.Enums;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 0;
        public bool showHome { get; set; } = false;
        public Status Status { get; set; } = Status.InActive;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int? LanguageId { get; set; }
        public Guid CreatedById { get; set; }
        public int CategoryId { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public List<HomeArticle> HomeArticles { get; set; } = new List<HomeArticle>();

        public Category Category { get; set; }
        public Language Language { get; set; }
        public AppUser CreatedBy { set; get; } 

    }
}
