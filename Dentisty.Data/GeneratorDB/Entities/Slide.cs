using Dentistry.Data.GeneratorDB.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Slide
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Url { set; get; }
        public int? ImageId { set; get; }
        public Guid UserId { set; get; }
        public AppUser CreatedBy { set; get; }
        public Image Image { get; set; }
        public int SortOrder { get; set; }
        public Status Status { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}