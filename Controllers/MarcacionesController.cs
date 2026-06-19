using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        // GET: api/Marcaciones
        // Muestra el historial completo unificado por filas diarias
        [HttpGet]
        public async Task<IActionResult> GetMarcaciones()
        {
            try
            {
                var marcaciones = await _context.Marcaciones
                    .Include(m => m.Empleado)
                    .AsNoTracking()
                    .OrderByDescending(m => m.Fecha)
                    .ToListAsync();

                return Ok(marcaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las marcaciones: {ex.Message}");
            }
        }

        // POST: api/Marcaciones
        // Guarda el formulario consolidado de la web en una sola fila por día
        [HttpPost]
        public async Task<IActionResult> PostMarcacion([FromBody] Marcacion marcacion)
        {
            if (marcacion == null)
            {
                return BadRequest("Datos de marcación inválidos.");
            }

            try
            {
                // Validación estricta para evitar duplicidad del mismo empleado el mismo día
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
    }
}