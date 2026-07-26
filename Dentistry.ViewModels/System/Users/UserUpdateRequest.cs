using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dentistry.ViewModels.System.Users
{
    public class UserUpdateRequest
    {
        public Guid Id { get; set; }

        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Hòm thư")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}