using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Entidades
{
    public class WorkArea
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre debe tener menos de 50 caracteres.")]
        [Remote("IsNameAvailable", "WorkAreas", AdditionalFields = "Id,Name", ErrorMessage = "Ya existe un área de trabajo con este nombre.")]
        public string Name { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }
        public List<Employee> Employees { get; set; }
    }
}
