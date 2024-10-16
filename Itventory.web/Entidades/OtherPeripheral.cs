using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itventory.web.Entidades
{
    public class OtherPeripheral
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(30, ErrorMessage = "El Nombre debe tener menos de 30 caracteres.")]
        [Remote("IsNameAvailable", "OtherPeripherals", AdditionalFields = "Id,Name", ErrorMessage = "Ya existe un Periferico con este nombre.")]
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
        //una workstation puede tener muchos perifericos
        public HashSet<WorkStation> WorkStations { get; set; }
    }
}
