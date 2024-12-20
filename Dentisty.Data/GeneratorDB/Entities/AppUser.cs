﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Dob { get; set; }
        public int? AvatarId { get; set; }
        public Image Avatar { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Slide> Slides { get; set; } = new List<Slide> { };
    }
}
