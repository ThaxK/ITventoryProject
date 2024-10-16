using Itventory.web.Entidades;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class DeviceModel
    {
        /// <summary>
        /// Id de dispositivo. Solo es necesario al editar
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id de tipo de dispositivo
        /// </summary>
        [Required]
        public int? DeviceTypeId { get; set; }
        /// <summary>
        /// Id de la marca del dispositivo 
        /// </summary>
        [Required]
        public int? DeviceBrandId { get; set; }
        /// <summary>
        /// Modelo del dispositivo
        /// </summary>
        [Required]
        public string Model { get; set; }
        /// <summary>
        /// La Serie debe tener menos de 40 caracteres y mas de 5 caracteres
        /// </summary>
        [Required]
        public string Series { get; set; }
        /// <summary>
        /// Id del procesador del dispositivo
        /// </summary>
        [Required]
        public int ProcessorId { get; set; }
        /// <summary>
        /// Cantidad de ram del dispositivo
        /// </summary>
        [Required]
        public string Ram { get; set; }
        /// <summary>
        /// capacidad del disco solido del dispositivo
        /// </summary>
        public int SolidStateDrive { get; set; }
        /// <summary>
        /// capacidad del disco mecanico del dispositivo
        /// </summary>
        public int HardDiskDrive { get; set; }
        /// <summary>
        /// solo si incluye. Id de la licencia de windows del dispositivo
        /// </summary>
        public int? WindowsLicenseId { get; set; }
        /// <summary>
        /// solo si incluye. Id de la licencia de antivirus del dispositivo
        /// </summary>
        public int? AntivirusLicenseId { get; set; }
        /// <summary>
        /// Status del dispositivo
        /// Disponible = 1,
        /// Asignado = 2,
        /// Pendiente = 3,
        /// </summary>
        public Status Status { get; set; }
    }
}
