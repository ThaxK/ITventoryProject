using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class OfficeSuiteModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre de la suite, maximo 50 caracteres
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// Serial, maximo 100 caracteres
        /// </summary>
        [Required]
        public string Series { get; set; }
        /// <summary>
        /// Fecha inicial de la suite de office
        /// </summary>
        [Required]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Fecha de vencimiento de la suite de office
        /// </summary>
        [Required]
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// Lista de Id's de los empleados que hacen parte de la suite de office. Maximo 5 empleados
        /// </summary>
        [Required]
        public List<int> SelectedEmployeeIds { get; set; } = new List<int>();
    }
}
