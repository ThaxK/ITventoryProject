using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class Subcategory
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La Categoria es Obligatoria.")]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(100, ErrorMessage = "El Nombre debe tener menos de 100 caracteres.")]
        [Remote("IsNameAvailable", "Subcategories", AdditionalFields = "Id,Name,CategoryId", ErrorMessage = "Ya existe un Recurso con este nombre.")]
        public string Name { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }


        //relaciones

        //propiedades de navegacion 
        public List<SoftwareLicense> SoftwareLicenses { get; set; } = new List<SoftwareLicense>();
        public List<Peripheral> PeripheralTypes { get; set; } = new List<Peripheral>();
        public List<Peripheral> PeripheralBrands { get; set; } = new List<Peripheral>();
        public List<Device> DeviceTypes { get; set; } = new List<Device>();
        public List<Device> DeviceBrands { get; set; } = new List<Device>();
        public List<Device> Processors { get; set; } = new List<Device>();
    }
}
