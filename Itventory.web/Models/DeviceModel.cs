using Itventory.web.Entidades;

namespace Itventory.web.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? DeviceBrandId { get; set; }
        public string Model { get; set; }
        public string Series { get; set; }
        public int ProcessorId { get; set; }
        public string Ram { get; set; }
        public int SolidStateDrive { get; set; }
        public int HardDiskDrive { get; set; }
        public int? WindowsLicenseId { get; set; }
        public int? AntivirusLicenseId { get; set; }
        public Status Status { get; set; }
    }
}
