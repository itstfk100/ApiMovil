using ApiMovil.Data;
using ApiMovil.Models; // Ajusta al namespace de tu proyecto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ApiMovil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HorariosController : ControllerBase
    {
        private readonly AppDbContext _context; // Ajusta al nombre de tu DbContext

        public HorariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Horarios
        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _context.Horarios.ToListAsync();
            return Ok(horarios);
        }

        // POST: api/Horarios
        [HttpPost]
        public async Task<IActionResult> PostHorario([FromBody] Horario horario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Horario creado con éxito", id = horario.IdHorario });
        }
    }
}