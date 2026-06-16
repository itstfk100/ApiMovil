using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarcacionPro.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpleadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpleadosController(
            AppDbContext context
        )
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> Listar()
        {
            return await _context.Empleados.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> Buscar(int id)
        {
            var empleado =
                await _context.Empleados.FindAsync(id);

            if (empleado == null)
                return NotFound();

            return empleado;
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(
            Empleado empleado
        )
        {
            _context.Empleados.Add(empleado);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}