using Dentistry.Data.GeneratorDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dentisty.Data.GeneratorDB.Entities
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string? Profile { get; set; }
        public string? Description { get; set; }
        public DateTime Dob {  get; set; }
        public string Position { get; set; }
        public string? PositionExtent {  get; set; }
        public int? ImageId { get; set; }
        public ImageFile Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
