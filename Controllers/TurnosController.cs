using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ApiMovil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TurnosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurnosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTurnos()
        {
            var turnos = await _context.Turnos
                .Include(t => t.HorarioLunes)
                .Include(t => t.HorarioMartes)
                .Include(t => t.HorarioMiercoles)
                .Include(t => t.HorarioJueves)
                .Include(t => t.HorarioViernes)
                .Include(t => t.HorarioSabado)
                .Include(t => t.HorarioDomingo)
                .ToListAsync();

            return Ok(turnos);
        }

        [HttpPost]
        public async Task<IActionResult> PostTurno([FromBody] Turno turno)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            turno.Estado = true;
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno configurado con éxito", id = turno.IdTurno });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurno(int id, [FromBody] Turno turno)
        {
            if (id != turno.IdTurno) return BadRequest();

            _context.Entry(turno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurno(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            // Soft Delete en lugar de Remove para evitar errores de integridad con Planificaciones
            turno.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno desactivado correctamente" });
        }
    }
}
