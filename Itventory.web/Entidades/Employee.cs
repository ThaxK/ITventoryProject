using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(30, ErrorMessage = "Maximo 30 caracteres")]
        [PersonalData]
        public string Name { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(30, ErrorMessage = "Maximo 30 caracteres")]
        [PersonalData]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(10, ErrorMessage = "Maximo 10 caracteres")]
        [Remote("IsDocumentAvailable", "Employees", AdditionalFields = "Id,DocumentNumber", ErrorMessage = "Ya existe un empleado con ese numero de documento.")]
        [PersonalData]
        public string DocumentNumber { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Debe tener 10 caracteres.")]
        [StringLength(10, ErrorMessage = "Debe tener 10 caracteres.")]
        [PersonalData]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [EmailAddress(ErrorMessage = "Correo electronico no valido.")]
        [PersonalData]
        public string Email { get; set; }
        [StringLength(500, ErrorMessage = "Maximo 500 caracteres")]
        [PersonalData]
        [Required(ErrorMessage = "Requerido.")]
        public string Address { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        //relaciones
        [NotMapped]
        public string FullName => $"{Name} {LastName} {DocumentNumber}";
        public List<OfficeSuite> OfficeSuites { get; set; } = new List<OfficeSuite>();

        //un empleado tiene un area de trabajo
        [Required(ErrorMessage = "Requerido.")]
        public int WorkAreaId { get; set; }
        [ForeignKey("WorkAreaId")]
        public WorkArea WorkArea { get; set; }

        //Propiedades de navegacion
        public List<WorkStation> WorkStations { get; set; }
        public List<Product> Products { get; set; }
    }
}
