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

        [HttpPost]
        public async Task<IActionResult> PostMarcacion([FromBody] Marcacion marcacion)
        {
            if (marcacion == null)
            {
                return BadRequest("Datos de marcación inválidos.");
            }

            try
            {
                bool yaExiste = await _context.Marcaciones
                    .AnyAsync(m => m.IdEmpleado == marcacion.IdEmpleado && m.Fecha.Date == marcacion.Fecha.Date);

                if (yaExiste)
                {
                    return BadRequest("El trabajador ya cuenta con un registro de asistencia para la fecha seleccionada.");
                }

                _context.Marcaciones.Add(marcacion);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Asistencia guardada correctamente en la Base de Datos." });
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
