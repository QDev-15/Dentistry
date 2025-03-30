namespace Dentistry.Data.GeneratorDB.Entities
{
    public class Logger
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserId { get; set; }
        public string IdAddress { get; set; }
    }
}
