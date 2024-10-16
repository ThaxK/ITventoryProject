using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itventory.web.Entidades
{
    public class WorkStation
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }

        //Relaciones

        [Required(ErrorMessage = "El Empleado es Obligatorio.")]
        [Remote("IsNameAvailable", "WorkStations", AdditionalFields = "Id,EmployeeId", ErrorMessage = "Ya existe una Estacion de Trabajo con este Empleado.")]
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [Required(ErrorMessage = "El Computador es Obligatorio.")]
        public int ComputerDeviceId { get; set; }
        [ForeignKey("ComputerDeviceId")]
        public Device ComputerDevice { get; set; }
        public int? SmartPhoneDeviceId { get; set; }
        [ForeignKey("SmartPhoneDeviceId")]
        public Device SmartPhoneDevice { get; set; }

        //Propiedades de navegacion
        [NotMapped]
        public List<int> PeripheralsIds { get; set; }
        public List<Peripheral> Peripherals { get; set; } = new List<Peripheral>();
        [NotMapped]
        public List<int> OtherPeripheralsIds { get; set; }
        public List<OtherPeripheral> OtherPeripherals { get; set; } = new List<OtherPeripheral>();
        public List<Act> Acts { get; set; } = new List<Act>();

    }
}
