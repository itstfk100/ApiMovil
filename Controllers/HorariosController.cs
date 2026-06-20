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
    public class HorariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HorariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHorarios()
        {
            var horarios = await _context.Horarios.ToListAsync();
            return Ok(horarios);
        }

        [HttpPost]
        public async Task<IActionResult> PostHorario([FromBody] Horario horario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Horario creado con éxito", id = horario.IdHorario });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, [FromBody] Horario horario)
        {
            if (id != horario.IdHorario) return BadRequest();

            _context.Entry(horario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Horario actualizado con éxito" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return NotFound();

            horario.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Horario desactivado con éxito" });
        }
    }
}
