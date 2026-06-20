using System;
using System.ComponentModel.DataAnnotations;

namespace ApiMovil.Models
{
    public class Horario
    {
        [Key]
        public int IdHorario { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreHorario { get; set; } = string.Empty;

        [Required]
        public TimeSpan HoraEntrada { get; set; }

        [Required]
        public TimeSpan HoraSalida { get; set; }

        public TimeSpan? HoraRefrigerio { get; set; }

        public TimeSpan? HoraFinRefrigerio { get; set; }

        public int ToleranciaMinutos { get; set; } = 0;

        public bool Estado { get; set; } = true;
    }
}
