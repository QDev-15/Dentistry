using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? TimeZone { set; get; }
        public string? IdLogin { set; get; }

        public DateTime Dob { get; set; }
        public int? AvatarId { get; set; }
        public ImageFile Avatar { get; set; }
        public List<Contact> Missions { get; set; }
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Slide> Slides { get; set; } = new List<Slide> { };
    }
}
