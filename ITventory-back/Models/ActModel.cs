using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class ActModel
    {
        /// <summary>
        /// Id de acta. Solo es necesario al editar
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// El Nombre de la acta.
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// El Id de la workStation.
        /// </summary>
        [Required]
        public int WorkStationId { get; set; }
        /// <summary>
        /// El archivo de la acta en formato PDF.
        /// </summary>
        [Required]
        public IFormFile File { get; set; }
    }
}
