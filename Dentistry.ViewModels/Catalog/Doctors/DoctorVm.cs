

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Dentistry.ViewModels.Catalog.Doctors
{
    public class DoctorVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        public DateTime Dob { get; set; }
        public string Position { get; set; }
        public string PositionExtent { get; set; }
        public ImageVm Avatar { get; set; }

        public IFormFile formFile { get; set; }
    }
}
