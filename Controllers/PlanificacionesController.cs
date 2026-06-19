using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ApiMovil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanificacionesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlanificacionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Planificaciones
        [HttpGet]
        public async Task<IActionResult> GetPlanificaciones()
        {
            var planificaciones = await _context.Planificaciones
                .Include(p => p.Empleado)
                .Include(p => p.Turno)
                .ToListAsync();

            return Ok(planificaciones);
        }

        // POST: api/Planificaciones
        [HttpPost]
        public async Task<IActionResult> PostPlanificacion([FromBody] Planificacion planificacion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (planificacion.FechaInicio > planificacion.FechaFin)
            {
                return BadRequest("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }

            _context.Planificaciones.Add(planificacion);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Empleado planificado con éxito" });
        }
    }
}