using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Enums
{
    public enum CategoryPosition
    {
        [Display(Name = "Không xác định")]
        None,
        [Display(Name = "Menu Trái")]
        MenuLef,
        [Display(Name = "Menu Phải")]
        MenuRight
    }
}
