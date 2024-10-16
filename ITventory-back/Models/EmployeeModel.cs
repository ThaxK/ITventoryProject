using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class EmployeeModel
    {
        /// <summary>
        /// Id del empleado. Solo se requiere al editar
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nombre, maximo 30 caracteres
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Apellidos, maximo 30 caracteres
        /// </summary>
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Numero de documento, maximo 10 caracteres
        /// </summary>
        [Required]
        public string DocumentNumber { get; set; }
        /// <summary>
        /// Numero de telefono, debe tener 10 caracteres
        /// </summary>
        [Required]
        public string Phone { get; set; }
        /// <summary>
        /// Correo electronico
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Direccion, maximo 500 caracteres
        /// </summary>
        [Required]
        public string Address { get; set; }
        /// <summary>
        /// Id del area de trabajo al que pertenece
        /// </summary>
        [Required]
        public int WorkAreaId { get; set; }
    }
}
