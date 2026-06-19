using System;

namespace ApiMovil.Models
{
    public class ReporteMarcacionDto
    {
        public int IdEmpleado { get; set; }
        public string Trabajador { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty; // "yyyy-MM-dd"
        public string DiaSemana { get; set; } = string.Empty; // "Lunes", "Martes", etc.
        public string Turno { get; set; } = string.Empty; // Nombre del turno planificado
        public string? HoraEntrada { get; set; } // Hora real que marcó Entrada
        public string? HoraSalida { get; set; } // Hora real que marcó Salida
        public string TiempoLaborado { get; set; } = "00:00"; // Cálculo de la diferencia
    }
}