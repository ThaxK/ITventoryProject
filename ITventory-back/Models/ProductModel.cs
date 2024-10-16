using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre del producto
        /// </summary>
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener menos de 50 caracteres.")]
        public string ProductName { get; set; }
        /// <summary>
        /// Nombre de usuario 
        /// </summary>
        [Required(ErrorMessage = "Requerido.")]
        [StringLength(50, ErrorMessage = "Debe tener menos de 50 caracteres.")]
        public string UserName { get; set; }
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [Required]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [Required]
        public DateTime? FinishDate { get; set; }
        /// <summary>
        /// Id del empleado asignado
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }
    }
}
