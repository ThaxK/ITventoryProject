using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class WorkStationModel
    {
        public int Id { get; set; }
        /// <summary>
        /// Id del empleado
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }
        /// <summary>
        /// Id del computador ligado a la estación de trabajo
        /// </summary>
        [Required]
        public int ComputerDeviceId { get; set; }
        /// <summary>
        /// Id del smartphone ligado a la estación de trabajo.
        /// </summary>
        public int? SmartPhoneDeviceId { get; set; }
        /// <summary>
        /// Lista de Id's de perifericos ligados a la estación de trabajo
        /// </summary>
        public List<int> PeripheralsIds { get; set; }
        /// <summary>
        /// Lista de Id's de perifericos mecanicos ligados a la estación de trabajo
        /// </summary>
        public List<int> OtherPeripheralsIds { get; set; }
    }
}
