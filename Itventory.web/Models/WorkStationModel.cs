namespace Itventory.web.Models
{
    public class WorkStationModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ComputerDeviceId { get; set; }
        public int? SmartPhoneDeviceId { get; set; }
        public List<int> PeripheralsIds { get; set; }
        public List<int> OtherPeripheralsIds { get; set; }
    }
}
