using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovil.Models
{
    public class Planificacion
    {
        [Key]
        public int IdPlanificacion { get; set; }

        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        public int IdTurno { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public bool Estado { get; set; } = true;

        [ForeignKey("IdEmpleado")]
        public Empleado? Empleado { get; set; }

        [ForeignKey("IdTurno")]
        public Turno? Turno { get; set; }
    }
}