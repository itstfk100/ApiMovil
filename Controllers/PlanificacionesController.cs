using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        [HttpGet]
        public async Task<IActionResult> GetPlanificaciones()
        {
            var planificaciones = await _context.Planificaciones
                .Include(p => p.Empleado)
                .Include(p => p.Turno)
                .ToListAsync();

            return Ok(planificaciones);
        }

        [HttpPost]
        public async Task<IActionResult> PostPlanificacion([FromBody] Planificacion planificacion)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Planificaciones.Add(planificacion);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Planificación creada con éxito" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlanificacion(int id, [FromBody] Planificacion planificacion)
        {
            if (id != planificacion.IdPlanificacion) return BadRequest();

            _context.Entry(planificacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Planificación actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlanificacion(int id)
        {
            var planificacion = await _context.Planificaciones.FindAsync(id);
            if (planificacion == null) return NotFound();

            planificacion.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Planificación desactivada correctamente" });
        }
    }
}
