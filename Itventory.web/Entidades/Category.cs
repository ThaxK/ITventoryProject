using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Entidades
{
    public class Category
    {
        public int Id { get; set; }
        [StringLength(150)]
        [Required]
        public string Name { get; set; }

        //propiedades de navegacion
        public List<Subcategory> Subcategories { get; set; } = new List<Subcategory>();
    }
}
