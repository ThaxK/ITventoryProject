using System.ComponentModel.DataAnnotations;

namespace Itventory.web.Models
{
    public class UsuarioVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
    }
}
