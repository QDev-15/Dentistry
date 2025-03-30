namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Slide
    {
        public int Id { set; get; }
        public string? Caption { set; get; }
        public string Name { set; get; }
        public string? SubName { set; get; }
        public string? Description { set; get; }
        public string? Url { set; get; }
        public int? ImageId { set; get; }
        public Guid UserId { set; get; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public AppUser CreatedBy { set; get; }
        public ImageFile Image { get; set; }
    }
}