namespace Itventory.web.Models
{
    public class ActModelDownload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int WorkStationId { get; set; }
        public IFormFile File { get; set; }
    }
}
