using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Itventory.web.Entidades
{
    public class Device
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La Categoria es obligatoria.")]
        [ForeignKey("DeviceTypeId")]
        public int? DeviceTypeId { get; set; }
        public Subcategory DeviceType { get; set; }
        [Required(ErrorMessage = "La Marca es obligatoria.")]
        [ForeignKey("DeviceBrandId")]
        public int? DeviceBrandId { get; set; }
        public Subcategory DeviceBrand { get; set; }
        [Required(ErrorMessage = "El Modelo es obligatorio.")]
        [StringLength(30, ErrorMessage = "El Modelo debe tener menos de 30 caracteres.")]
        public string Model { get; set; }
        [Required(ErrorMessage = "La Serie es obligatoria.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "La Serie debe tener menos de 40 caracteres y mas de 5 caracteres.")]
        [Remote("IsNameAvailable", "Devices", AdditionalFields = "Id,Series,DeviceTypeId", ErrorMessage = "Ya existe una Serie con estos Caracteres.")]
        public string Series { get; set; }
        [Required(ErrorMessage = "El Procesador es obligatorio.")]
        [ForeignKey("ProcessorId")]
        public int ProcessorId { get; set; }
        public Subcategory Processor { get; set; }
        [Required(ErrorMessage = "La Ram es obligatoria.")]
        public string Ram { get; set; }
        [Required(ErrorMessage = "El Disco de Estado Solido es obligatorio.")]
        public int SolidStateDrive { get; set; }
        [Required(ErrorMessage = "El Disco Mecanico es obligatorio.")]
        public int HardDiskDrive { get; set; }
        public int? WindowsLicenseId { get; set; }
        public int? AntivirusLicenseId { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        [ForeignKey("WindowsLicenseId")]
        public SoftwareLicense WindowsLicense { get; set; }
        [ForeignKey("AntivirusLicenseId")]
        public SoftwareLicense AntivirusLicense { get; set; }

        //propiedades de navegacion
        public List<WorkStation> ComputerDevices { get; set; }
        public List<WorkStation> SmartPhoneDevices { get; set; }

    }
}
