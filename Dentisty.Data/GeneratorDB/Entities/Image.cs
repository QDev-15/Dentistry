using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class ImageFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string ThumbPath { get; set; }
        public string Type { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
        public List<Doctor> DoctorsBackground { get; set; } = new List<Doctor>();
        public List<AppUser> AppUsers { get; set; }
        public List<Category> Categories { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();
        public List<Slide> Slides { get; set; } = new List<Slide>();
    }
}
