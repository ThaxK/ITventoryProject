using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electronico valido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Recuerdame")]
        public bool Recuerdame { get; set; }
    }
}

