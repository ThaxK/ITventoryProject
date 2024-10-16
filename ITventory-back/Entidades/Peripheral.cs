using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class Peripheral
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Modelo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El Modelo debe tener menos de 30 caracteres.")]
        public string Model { get; set; }
        [Required(ErrorMessage = "La Serie es obligatorio.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Serie debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        [Remote("IsNameAvailable", "Peripherals", AdditionalFields = "Id,Series,PeripheralTypeId", ErrorMessage = "Ya existe una Serie con estos Caracteres.")]
        public string Series { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero pesos")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "La fecha de compra es requerida.")]
        public DateTime PurchaseDate { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        //relaciones 
        //un periferico tiene un tipo de periferico
        [Required(ErrorMessage = "La Categoria es Obligatorio.")]
        [ForeignKey("PeripheralTypeId")]
        public int PeripheralTypeId { get; set; }
        public Subcategory PeripheralType { get; set; }

        //un periferico tiene un tipo de marca
        [Required(ErrorMessage = "La Marca es Obligatoria.")]
        [ForeignKey("PeripheralBrandId")]
        public int PeripheralBrandId { get; set; }
        public Subcategory PeripheralBrand { get; set; }

        //muchos perifericos tienen muchas workstations 
        public HashSet<WorkStation> WorkStations { get; set; }

        //Propiedades de navegacion.
    }
}