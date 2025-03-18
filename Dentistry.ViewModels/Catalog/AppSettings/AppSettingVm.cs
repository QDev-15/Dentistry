using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Catalog.AppSettings
{
    public class AppSettingVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CompanyName { set; get; }
        public string? CompanyAddress { set; get; }
        public string? CompanyEmail { set; get; }
        public string? CompanyWebsite { set; get; }
        public string? CompanyPhone { set; get; }
        public string? CompanyTitle { set; get; }
        public string? BranchesTitle { set; get; }

        public string? HotlineHaNoi { get; set; }
        public string? ZaloHotline { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }
        public string? Tiktok { get; set; }
        public string? StartWork { get; set; }
        public string? EndWork { get; set; }
        public string? NewsListTitle { get; set; }
        public string? CategoryListTitle { get; set; }
        public string? CategoryListSubTitle { get; set; }
        public string? CategoryListProductTitle { get; set; }
        public string? CategoryListProductSubTitle { get; set; }
        public string? DoctorListTitle { get; set; }
        public string? DoctorListSubTitle { get; set; }

        public bool ShowToolBarTop { get; set; } = true;
        public bool ShowContactList { get; set; } = true;
        public bool ShowCategoryList { get; set; } = true;
        public bool ShowCategoryProductList { get; set; } = true;
        public bool ShowDoctorSlideList { get; set; } = true;
        public bool ShowArtileSlideList { get; set; } = true;
        public bool ShowProductList { get; set; } = true;
        public bool ShowNewsList { get; set; } = true;
        public bool ShowFeedbackList { get; set; } = true;
        public bool TrackVisitors { get; set; } = true;
        public string? Categories { set; get; } = "";
        public string? CategoryProducts { set; get; } = "";
        public string? Doctors { set; get; } = "";
        public string? Products { set; get; } = "";
        public string? Articles { set; get; } = "";
        public string? News { set; get; } = "";
        public string? Feedbacks { set; get; } = "";
    }
}

