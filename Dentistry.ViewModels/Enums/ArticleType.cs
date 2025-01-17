using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Enums
{
    public enum ArticleType
    {
        [Display(Name = "Bài Viết", GroupName = "bai-viet")]
        Article,
        [Display(Name = "Phản hồi", GroupName = "phan-hoi")]
        FeedBack,
        [Display(Name = "Tin tức", GroupName = "tin-tuc")]
        News,
        [Display(Name = "Sản phẩm", GroupName = "san-pham")]
        Products
    }
}
