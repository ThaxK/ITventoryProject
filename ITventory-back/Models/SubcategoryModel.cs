using Itventory.web.Entidades;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class SubcategoryModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre de la subcategoria
        /// </summary>
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(100, ErrorMessage = "El Nombre debe tener menos de 100 caracteres.")]
        public string Name { get; set; }
        [Required]
        public Status Status { get; set; }
        /// <summary>
        /// Id de la categoria a la que pertenece
        /// </summary>
        [Required(ErrorMessage = "La Categoria es Obligatoria.")]
        public int CategoryId { get; set; }
    }
}
