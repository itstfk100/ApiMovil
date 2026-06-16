using System.ComponentModel.DataAnnotations;

namespace ApiMovil.Models
{
    public class Empleado
    {
        [Key]
        public int IdEmpleado { get; set; }

        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string DNI { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}