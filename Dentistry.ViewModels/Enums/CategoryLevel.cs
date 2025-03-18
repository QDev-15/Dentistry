using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Enums
{
    public enum CategoryLevel
    {
        [Display(Name = "Danh mục Cấp 1")]
        Level1,
        [Display(Name = "Danh mục Cấp 2")]
        Level2,
        [Display(Name = "Danh mục Cấp 3")]
        Level3
    }
}
