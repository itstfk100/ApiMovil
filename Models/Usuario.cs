using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovil.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Column("Usuario")]
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;

        public int IdEmpleado { get; set; }

        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}