namespace Itventory.web.Models
{
    public class ActModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WorkStationId { get; set; }
        public IFormFile File { get; set; }
    }
}
