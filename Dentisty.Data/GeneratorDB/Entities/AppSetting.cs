using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class AppSetting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? HotlineHaNoi { get; set; }
        public string? ZaloHotline { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Twitter {  get; set; }
        public string? Youtube { get; set; }
        public string? StartWork {  get; set; }
        public string? EndWork {  get; set; }
        public bool ShowToolBarTop { get; set; } = true;
        public bool ShowContactList { get; set;} = true;
        public bool ShowCategoryList { get; set;} = true;
        public bool ShowDoctorlideList { get; set;} = true;
        public bool ShowArtileSlideList { get; set;} = true;
        public bool ShowProductList { get; set; } = true;
        public bool ShowNewsList { get; set; } = true;
        public bool ShowFeedbackList { get; set; } = true;  
    }
}
