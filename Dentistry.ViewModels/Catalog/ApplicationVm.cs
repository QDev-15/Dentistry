using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Branches;
using Dentistry.ViewModels.Catalog.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog
{
    public class ApplicationVm
    {
        public AppSettingVm Settings { get; set; } = new AppSettingVm();
        public List<CategoryVm> Categories { get; set; } = new List<CategoryVm>();
        public List<BranchesVm> Branches { get; set; } = new List<BranchesVm>();
    }
}
