using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OfficeSuiteModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Series { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? FinishDate { get; set; }

        public List<int> SelectedEmployeeIds { get; set; } = new List<int>();
    }
}
