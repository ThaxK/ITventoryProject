using Itventory.web.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class OtherPeripheralModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Nombre del periferico, maximo 30 caracteres
        /// </summary>
        [Required]
        public string Name { get; set; }
        [Required]
        public Status Status { get; set; }
    }
}
