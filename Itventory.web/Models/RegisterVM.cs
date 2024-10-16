using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "El Email es requerido.")]
        [EmailAddress(ErrorMessage = "El campo debe ser un Correo Electrónico válido.")]
        [Remote("IsEmailAvailable", "Usuarios", AdditionalFields = "Id", ErrorMessage = "Ya existe un usuario con ese Correo electrónico.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña es requerida.")]
        [StringLength(255, ErrorMessage = "La {0} debe tener al menos {2} caracteres, una letra mayuscula, una letra minuscula y un caracter especial.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "La contraseña debe tener al menos una letra minúscula, una letra mayúscula, un dígito y un carácter especial.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debes confirmar la contraseña.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Debes seleccionar un Rol.")]
        public string Rol { get; set; }
        
        public List<string> RolesDisponibles { get; set; }
    }
}
