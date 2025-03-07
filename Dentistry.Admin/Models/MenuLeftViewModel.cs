using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.System.Users;

namespace Dentistry.Admin.Models
{
    public class MenuLeftViewModel
    {
        public List<CategoryVm> Categories { get; set; } = new List<CategoryVm>();
        public UserVm CurrentUser { get; set; }
    }
}
