using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener menos de 50 caracteres.")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener menos de 50 caracteres.")]
        public string UserName { get; set; }
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

        //Relaciones
        //Una suite de office tiene una relacion de cero a muchos... 
        [Required(ErrorMessage = "Requerido.")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
