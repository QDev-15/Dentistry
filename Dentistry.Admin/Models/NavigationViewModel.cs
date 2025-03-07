using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dentistry.Admin.Models
{
    public class NavigationViewModel
    {
        public List<SelectListItem> Languages { get; set; }

        public string CurrentLanguageId { get; set; }

        public string ReturnUrl { set; get; }
        public UserVm CurrentUser { get; set; }
        public bool ShowLeftNavBar { get; set; } = true;
    }
}