using Itventory.web.Entidades;

namespace WebApplication1.Models
{
    public class OfficeSuiteModelDetails
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Series { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public Status Status { get; set; }

        public int? Stock { get; set; }

        public List<int> SelectedEmployeeIds { get; set; } = new List<int>();
    }
}
