using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class OfficeSuite
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre de usuario es requerido.")]
        [StringLength(50, ErrorMessage = "Maximo 50 caracteres")]
        [Remote("IsUserNameAvailable", "OfficeSuites", AdditionalFields = "Id,UserName", ErrorMessage = "Ya existe una licencia con ese nombre de usuario.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "El Serial es requerido.")]
        [StringLength(100, ErrorMessage = "Maximo 100 caracteres")]
        public string Series { get; set; }
        public int? Stock { get; set; }
        [Required(ErrorMessage = "La fecha inicial es requerida.")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "La fecha de vencimiento es requerida.")]
        public DateTime? FinishDate { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        //relaciones
        //una suite de office puede tener 0 o muchos empleados (5) 
        [NotMapped]
        public string OfficeFullName => $"{UserName} -- Vence el: {FinishDate?.ToString("yyyy/MM/dd")}";
        [NotMapped]
        public List<int> SelectedEmployeeIds { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
