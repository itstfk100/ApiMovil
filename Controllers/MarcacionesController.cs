using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMovil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MarcacionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMarcaciones([FromQuery] int limit = 100)
        {
            try
            {
                var marcaciones = await _context.Marcaciones
                    .Include(m => m.Empleado)
                    .AsNoTracking()
                    .OrderByDescending(m => m.Fecha)
                    .ThenByDescending(m => m.HoraEntrada)
                    .Take(limit) // Limitamos para mejorar performance
                    .ToListAsync();

                return Ok(marcaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las marcaciones: {ex.Message}");
            }
        }

        // 🚀 MÉTODO OPTIMIZADO: Detecta automáticamente Entrada o Salida desde el Móvil
        [HttpPost]
        public async Task<IActionResult> PostMarcacion([FromBody] Marcacion marcacion)
        {
            if (marcacion == null)
            {
                return BadRequest("Datos de marcación inválidos.");
            }

            try
            {
                // 1. Buscamos si el empleado ya inició jornada el día de hoy
                var marcacionExistente = await _context.Marcaciones
                    .FirstOrDefaultAsync(m => m.IdEmpleado == marcacion.IdEmpleado && m.Fecha.Date == marcacion.Fecha.Date);

                // ==========================================
                // CASO A: ENTRADA (No hay marcas hoy)
                // ==========================================
                if (marcacionExistente == null)
                {
                    _context.Marcaciones.Add(marcacion);
                    await _context.SaveChangesAsync();
                    return Ok(new { mensaje = "Entrada registrada correctamente." });
                }

                // ==========================================
                // CASO B: SALIDA (Ya registró entrada hoy)
                // ==========================================
                else
                {
                    if (marcacionExistente.HoraSalida != null)
                    {
                        return BadRequest("El trabajador ya cuenta con un registro completo de Entrada y Salida para hoy.");
                    }

                    // Obtener la hora y fecha actuales del sistema
                    DateTime fechaHoy = DateTime.Now;
                    TimeSpan horaActualMarcacion = marcacion.HoraEntrada ?? fechaHoy.TimeOfDay;

                    // 1. Buscar la Planificación ACTIVA del empleado para la fecha actual
                    var planificacion = await _context.Planificaciones
                        .FirstOrDefaultAsync(p => p.IdEmpleado == marcacion.IdEmpleado
                                               && fechaHoy.Date >= p.FechaInicio.Date
                                               && fechaHoy.Date <= p.FechaFin.Date);

                    if (planificacion != null)
                    {
                        // 2. Buscar el Turno asociado a la planificación incluyendo sus horarios
                        var turno = await _context.Turnos
                        .FirstOrDefaultAsync(t => t.IdTurno == planificacion.IdTurno && t.Estado == true);

                        if (turno != null)
                        {
                            // 3. Determinar qué horario toca según el día de la semana de HOY
                            int? idHorarioDelDia = fechaHoy.DayOfWeek switch
                            {
                                DayOfWeek.Monday => turno.IdHorarioLunes,
                                DayOfWeek.Tuesday => turno.IdHorarioMartes,
                                DayOfWeek.Wednesday => turno.IdHorarioMiercoles,
                                DayOfWeek.Thursday => turno.IdHorarioJueves,
                                DayOfWeek.Friday => turno.IdHorarioViernes,
                                DayOfWeek.Saturday => turno.IdHorarioSabado,
                                DayOfWeek.Sunday => turno.IdHorarioDomingo,
                                _ => null
                            };

                            // 4. Si hay un horario asignado para este día de la semana, buscamos sus detalles
                            if (idHorarioDelDia.HasValue)
                            {
                                var horario = await _context.Horarios
                                .FirstOrDefaultAsync(h => h.IdHorario == idHorarioDelDia.Value && h.Estado == true);

                                if (horario != null)
                                {
                                    TimeSpan horaInicioRefrigerio = horario.HoraRefrigerio ?? TimeSpan.Zero;
                                    TimeSpan horaFinRefrigerio = horario.HoraFinRefrigerio ?? TimeSpan.Zero;

                                    if (horaActualMarcacion > horaFinRefrigerio)
                                    {
                                        marcacionExistente.InicioDescanso = horaInicioRefrigerio;
                                        marcacionExistente.FinDescanso = horaFinRefrigerio;
                                    }
                                }
                            }
                        }
                    }

                    marcacionExistente.HoraSalida = horaActualMarcacion;

                    if (!string.IsNullOrEmpty(marcacion.Foto))
                    {
                        marcacionExistente.Foto = marcacion.Foto;
                    }

                    _context.Entry(marcacionExistente).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok(new { mensaje = "Salida y refrigerio automático validados con éxito." });
                }
            }
            catch (Exception ex)
            {
                var errorInterno = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return StatusCode(500, $"Error en SQL Server: {errorInterno}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarcacion(int id, [FromBody] Marcacion marcacion)
        {
            if (id != marcacion.IdMarcacion) return BadRequest();

            _context.Entry(marcacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marcación actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarcacion(int id)
        {
            var marcacion = await _context.Marcaciones.FindAsync(id);
            if (marcacion == null) return NotFound();

            marcacion.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marcación desactivada correctamente" });
        }

        [HttpGet("Reporte")]
        public async Task<IActionResult> GetReporte()
        {
            var marcaciones = await _context.Marcaciones
                .Include(m => m.Empleado)
                .AsNoTracking()
                .ToListAsync();

            var reporte = marcaciones.Select(m => new {
                IdEmpleado = m.IdEmpleado,
                Trabajador = m.Empleado != null ? $"{m.Empleado.Nombres} {m.Empleado.Apellidos}" : "N/A",
                Fecha = m.Fecha.ToString("yyyy-MM-dd"),
                DiaSemana = m.Fecha.ToString("dddd"),
                HoraEntrada = m.HoraEntrada?.ToString(@"hh\:mm") ?? "--",
                HoraSalida = m.HoraSalida?.ToString(@"hh\:mm") ?? "--"
            });

            return Ok(reporte);
        }

        [HttpPost("Masivo")]
        public async Task<IActionResult> PostMasivo([FromBody] List<Marcacion> marcas)
        {
            foreach (var m in marcas)
            {
                _context.Marcaciones.Add(m);
            }
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Marcaciones masivas guardadas" });
        }
    }
}