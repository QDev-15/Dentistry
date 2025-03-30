using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentistry.ViewModels.Enums
{
    public enum CategoryType
    {
        [Display(Name = "Chưa xác định", GroupName = "default")]
        None,
        [Display(Name = "Dịch vụ", GroupName = "dich-vu")]
        Service,
        [Display(Name = "Phản hồi", GroupName = "phan-hoi")]
        FeedBack,
        [Display(Name = "Tin tức", GroupName = "tin-tuc")]
        News,
        [Display(Name = "Sản phẩm", GroupName = "san-pham")]
        Products,
        [Display(Name = "Hỗ trợ", GroupName = "ho-tro")]
        Support,
        [Display(Name = "Giới thiệu", GroupName = "gioi-thieu")]
        About,
        [Display(Name = "Tư vấn", GroupName = "tu-van")]
        advise,
    }
}
