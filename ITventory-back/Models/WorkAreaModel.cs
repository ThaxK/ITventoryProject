using Itventory.web.Entidades;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class WorkAreaModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre del area de trabajo
        /// </summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre debe tener menos de 50 caracteres.")]
        public string Name { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
