using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class EditVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "La conraseña es requerida.")]
        [DataType(DataType.Password)]
        [StringLength(255, ErrorMessage = "La {0} debe tener al menos {2} caracteres, una letra mayuscula, una letra minuscula y un caracter especial .", MinimumLength = 8)]
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
