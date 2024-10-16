using Itventory.web.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class PeripheralModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Id de tipo de periferico
        /// </summary>
        [Required]
        public int PeripheralTypeId { get; set; }
        /// <summary>
        /// Id de marca de periferico
        /// </summary>
        [Required]
        public int PeripheralBrandId { get; set; }
        /// <summary>
        /// Modelo, maximo 30 caracteres
        /// </summary>
        [Required]
        public string Model { get; set; }
        /// <summary>
        /// Serie, minimo 5 caracteres, maximo 40 caracteres
        /// </summary>
        [Required]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Serie debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        public string Series { get; set; }
        /// <summary>
        /// Precio, debe ser mayor a 0 pesos
        /// </summary>
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Status Status { get; set; }
        /// <summary>
        /// Fecha de la compra
        /// </summary>
        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
