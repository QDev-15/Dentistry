﻿using Dentistry.ViewModels.Catalog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dentistry.ViewModels.System.Users
{
    public class UserVm
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }

        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime Dob { get; set; }
        public string TimeZone { set; get; }
        public string IdLogin { set; get; }
        public ImageVm Avatar { get; set; }
        public IFormFile ImageFile { get; set; }

        public IList<string> Roles { get; set; }
        public string CoverImage
        {
            get
            {
                var value = "/assets/img/no-image.jpg";
                if (Avatar != null && Avatar.Id > 0)
                {
                    value = Avatar.ThumbPath ?? Avatar.Path;
                }
                return value;
            }
        }
    }
}