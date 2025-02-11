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
        public ArticleType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDraft { get; set; }
        public Guid CreatedById { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Tags {  get; set; }
        public List<Image> Images { get; set; } = new List<Image>();

        public Category Category { get; set; }
        public AppUser CreatedBy { set; get; }

    }
}
