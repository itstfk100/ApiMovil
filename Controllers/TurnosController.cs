using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        // GET: api/Turnos
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

        // POST: api/Turnos
        [HttpPost]
        public async Task<IActionResult> PostTurno([FromBody] Turno turno)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Turno configurado con éxito", id = turno.IdTurno });
        }
    }
}