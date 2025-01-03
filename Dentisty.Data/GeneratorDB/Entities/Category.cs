using Dentistry.ViewModels.Enums;
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
        public string? Description { set; get; }
        public int? ImageId { set; get; }
        public bool IsActive { get; set; }
        public bool IsParent { get; set; }
        public CategoryPosition? Position { set; get; }
        public int Sort {  set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Image Image { set; get; }
        public Category Parent { set; get; }
        public AppUser AppUser { set; get; }
        public List<Category> Categories { set; get; }
        public List<Article> Articles { set; get; }
        public List<CategoryTranslation> CategoryTranslations { get; set; }
    }
}
