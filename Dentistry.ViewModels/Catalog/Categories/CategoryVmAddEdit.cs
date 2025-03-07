using Dentistry.ViewModels.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.Categories
{
    public class CategoryVmAddEdit
    {
        public CategoryVm item { get; set; } = new CategoryVm();
        public List<CategoryVm> parrents { get; set; } = new List<CategoryVm>();
        public List<SelectListItem> CategoryPositions = new List<SelectListItem>();
        public List<SelectListItem> CategoryType = new List<SelectListItem>();
    }
}
