using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SoftwareLicenseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string ProductKey { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int SubcategoryId { get; set; }
    }
}
