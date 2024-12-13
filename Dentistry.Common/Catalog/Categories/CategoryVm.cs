using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVm
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Alias { set; get; }
        public int Sort { set; get; }
        public int? ParentId { set; get; }
        public Guid? UserId { set; get; }
        public bool IsActive { get; set; } = true;
        public bool IsSub { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Decription { set; get; }
        public CategoryVm? Parent { get; set; }
        public List<CategoryVm> ChildCategories { get; set; } = new List<CategoryVm>();
    }
}