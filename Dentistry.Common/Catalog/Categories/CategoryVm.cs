using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVm
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Alias { get; set; }

        public int? ParentId { get; set; }
        public CategoryVm Parent { get; set; }
        public List<CategoryVm> ChildCategories { get; set; }
    }
}