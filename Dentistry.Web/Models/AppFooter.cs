using Dentistry.ViewModels.Catalog.AppSettings;
using Dentistry.ViewModels.Catalog.Branches;

namespace Dentistry.Web.Models
{
    public class AppFooter
    {
        public AppSettingVm setting { get; set; }
        public List<BranchesVm> branches { get; set; }
    }
}
