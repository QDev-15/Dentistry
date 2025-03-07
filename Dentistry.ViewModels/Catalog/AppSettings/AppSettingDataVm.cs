using Dentistry.ViewModels.Catalog.Articles;
using Dentistry.ViewModels.Catalog.Categories;
using Dentistry.ViewModels.Catalog.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.AppSettings
{
    public class AppSettingDataVm
    {
        public AppSettingVm Setting { set; get; }
        public List<CategoryVm> Categories { set; get; } = new List<CategoryVm>();
        public List<ArticleVm> Products { set; get; } = new List<ArticleVm>();
        public List<ArticleVm> Articles { set; get; } = new List<ArticleVm>();
        public List<DoctorVm> Doctors { set; get; } = new List<DoctorVm>();
        public List<ArticleVm> News { set; get; } = new List<ArticleVm>();
        public List<ArticleVm> FeedBacks { set; get; } = new List<ArticleVm>();
    }
}
