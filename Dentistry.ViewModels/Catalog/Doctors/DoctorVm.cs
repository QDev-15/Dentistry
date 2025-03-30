

using Dentistry.Common;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Dentistry.ViewModels.Catalog.Doctors
{
    public class DoctorVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Profile { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        public DateTime Dob { get; set; }
        public string Position { get; set; }
        public string PositionExtent { get; set; }
        public ImageVm Avatar { get; set; }
        public ImageVm Background { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public IFormFile formFile { get; set; }
        public IFormFile backgroundFile { get; set; }
        public string BackgroundPath
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Background != null && Background.Id > 0)
                {
                    value = Background.Path;
                    //value = string.IsNullOrEmpty(Image.ThumbPath) ? Image.Path : Image.ThumbPath;
                }
                return value;
            }
        }
        public string AvatarPath
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Avatar != null && Avatar.Id > 0)
                {
                    value = Avatar.Path;
                    //value = string.IsNullOrEmpty(Image.ThumbPath) ? Image.Path : Image.ThumbPath;
                }
                return value;
            }
        }

    }
}
