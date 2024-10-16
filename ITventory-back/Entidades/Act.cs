using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Itventory.web.Entidades
{
    public class Act
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string URL { get; set; }
        public Status Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public void UpdateTimestamp()
        {
            UpdateAt = DateTime.Now;
        }
        //relaciones 
        [NotMapped]
        [Required(ErrorMessage = "El archivo pdf es obligatorio.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "La Estacion de Trabajo es obligatoria.")]
        [ForeignKey("WorkStationId")]
        public int WorkStationId { get; set; }
        public WorkStation WorkStation { get; set; }
    }
}
