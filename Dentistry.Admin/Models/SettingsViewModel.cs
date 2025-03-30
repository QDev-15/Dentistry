using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Doctors;
using Dentistry.ViewModels.Catalog.Slide;
using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Admin.Models
{
    public class SettingsViewModel
    {
        public SettingsViewModel() { }
        public List<CategoryVm> Categories { get; set; } = new List<CategoryVm>();
        public List<SlideVm> Slides { get; set; } = new List<SlideVm> ();
        public List<ArticleVm> Articles { get; set; } = new List<ArticleVm>();
        public List<DoctorVm> Doctors { get; set; } = new List<DoctorVm> ();
    }
}
