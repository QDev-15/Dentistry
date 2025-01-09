using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Enums
{
    public enum ArtisleType
    {
        [Display(Name = "Bài Viết")]
        Article,
        [Display(Name = "Phản hồi")]
        FeedBack,
        [Display(Name = "Tin tức")]
        News,
        [Display(Name = "Sản phẩm")]
        Products
    }
}
