using System.ComponentModel.DataAnnotations;

namespace ApiMovil.Models
{
    public class Marcacion
    {
        [Key]
        public int IdMarcacion { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaHora { get; set; }
        public string TipoMarcacion { get; set; } = string.Empty;
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
    }
}