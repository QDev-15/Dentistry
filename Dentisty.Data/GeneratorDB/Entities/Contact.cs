using Dentisty.Data.GeneratorDB.Entities;

namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Contact
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string? Email { set; get; }
        public string PhoneNumber { set; get; }
        public string Message { set; get; }
        public string? Note { set; get; }
        public bool IsActive { get; set; }
        public Guid? ProcessById { set; get; }
        public int? BranchesId { set; get; }
        public DateTime? TimeBook { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public AppUser ProcessBy { set; get; }
        public Branches Branches { set; get; }

    }
}
