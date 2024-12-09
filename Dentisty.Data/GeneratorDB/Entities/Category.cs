using Dentisty.Data.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Category
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Alias { set; get; }
        public int? ParentId { set; get; }
        public Guid? UserId { set; get; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Category Parent { set; get; }
        public AppUser AppUser { set; get; }
        public List<Category> Categories { set; get; }
        public List<Article> Articles { set; get; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
