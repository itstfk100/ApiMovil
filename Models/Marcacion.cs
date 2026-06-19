using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovil.Models
{
    public class Marcacion
    {
        [Key]
        public int IdMarcacion { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; } // Guarda el día (Ej: 2026-06-19)

        public TimeSpan? HoraEntrada { get; set; }
        public TimeSpan? HoraSalida { get; set; }
        public TimeSpan? InicioDescanso { get; set; }
        public TimeSpan? FinDescanso { get; set; }

        [Column(TypeName = "decimal(18, 7)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(18, 7)")]
        public decimal? Longitud { get; set; }

        public string? Foto { get; set; } // String Base64 para la foto del móvil

        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }
    }
}