using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMovil.Models
{
    public class Turno
    {
        [Key]
        public int IdTurno { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreTurno { get; set; } = string.Empty; // Ej: "Lunes a Viernes - Oficina"

        // Claves foráneas para cada día de la semana
        public int? IdHorarioLunes { get; set; }
        public int? IdHorarioMartes { get; set; }
        public int? IdHorarioMiercoles { get; set; }
        public int? IdHorarioJueves { get; set; }
        public int? IdHorarioViernes { get; set; }
        public int? IdHorarioSabado { get; set; }
        public int? IdHorarioDomingo { get; set; }

        // Propiedades de navegación para que EF Core pueda traer la información de las horas de cada día
        [ForeignKey("IdHorarioLunes")]
        public Horario? HorarioLunes { get; set; }

        [ForeignKey("IdHorarioMartes")]
        public Horario? HorarioMartes { get; set; }

        [ForeignKey("IdHorarioMiercoles")]
        public Horario? HorarioMiercoles { get; set; }

        [ForeignKey("IdHorarioJueves")]
        public Horario? HorarioJueves { get; set; }

        [ForeignKey("IdHorarioViernes")]
        public Horario? HorarioViernes { get; set; }

        [ForeignKey("IdHorarioSabado")]
        public Horario? HorarioSabado { get; set; }

        [ForeignKey("IdHorarioDomingo")]
        public Horario? HorarioDomingo { get; set; }
    }
}