using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class SoftwareLicense
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(30, ErrorMessage = "El Nombre debe tener menos de 30 caracteres.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "La Serie es Obligatoria.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Serie debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        [Remote("IsSerialAvailable", "SoftwareLicenses", AdditionalFields = "Id,Series,SubcategoryId", ErrorMessage = "Ya existe un Recurso con esta Serie.")]
        public string Series { get; set; }
        [Required(ErrorMessage = "La Llave es Obligatoria.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Llave debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        public string ProductKey { get; set; }
        [Required(ErrorMessage = "La Fecha Inicial es Obligatoria.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "La Fecha final es Obligatoria.")]
        public DateTime FinishDate { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        //relaciones
        //una licencia tiene una subcategoria
        [Required(ErrorMessage = "La Categoria es Obligatoria.")]
        [ForeignKey("SubcategoryId")]
        public int SubcategoryId { get; set; }
        public Subcategory Subcategory { get; set; }

        //propiedades de navegacion
        public List<Device> WindowsLicenses { get; set; } = new List<Device>();
        public List<Device> AntivirusLicenses { get; set; } = new List<Device>();
    }
}
