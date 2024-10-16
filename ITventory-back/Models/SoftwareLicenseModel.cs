using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class SoftwareLicenseModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre de la licencia
        /// </summary>
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(30, ErrorMessage = "El Nombre debe tener menos de 30 caracteres.")]
        public string Name { get; set; }
        /// <summary>
        /// Serie de la licencia
        /// </summary>
        [Required(ErrorMessage = "La Serie es Obligatoria.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Serie debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        public string Series { get; set; }
        /// <summary>
        /// Llave de la licencia
        /// </summary>
        [Required(ErrorMessage = "La Llave es Obligatoria.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Llave debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        public string ProductKey { get; set; }
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [Required]
        public DateTime FinishDate { get; set; }
        /// <summary>
        /// Id de la subcategoria
        /// </summary>
        [Required]
        public int SubcategoryId { get; set; }
    }
}
