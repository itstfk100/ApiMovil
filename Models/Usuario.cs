using System.ComponentModel.DataAnnotations;

namespace ApiMovil.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}