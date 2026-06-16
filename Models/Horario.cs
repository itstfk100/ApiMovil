using System.ComponentModel.DataAnnotations;

namespace ApiMovil.Models
{
    public class Horario
    {
        [Key]
        public int IdHorario { get; set; }
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraRefrigerio { get; set; }
        public TimeSpan HoraFinRefrigerio { get; set; }
        public int ToleranciaMinutos { get; set; }
    }
}
