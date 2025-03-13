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
        public string? HotlineHaNoi { get; set; }
        public string? ZaloHotline { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter { get; set; }
        public string? Youtube { get; set; }
        public string? StartWork { get; set; }
        public string? EndWork { get; set; }
        public bool ShowToolBarTop { get; set; }
        public bool ShowContactList { get; set; }
        public bool ShowCategoryList { get; set; }
        public bool ShowDoctorSlideList { get; set; }
        public bool ShowArtileSlideList { get; set; }
        public bool ShowProductList { get; set; }
        public bool ShowNewsList { get; set; }
        public bool ShowFeedbackList { get; set; }
        public bool TrackVisitors { get; set; } = true;
        public string Categories { set; get; } = "";
        public string Doctors { set; get; } = "";
        public string Articles { set; get; } = "";
        public string News { set; get; } = "";
        public string Feedbacks { set; get; } = "";
        public string Products { set; get; } = "";
    }
}
