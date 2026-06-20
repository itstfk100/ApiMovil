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

        public EmpleadosController(AppDbContext context)
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
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            return empleado;
        }

        [HttpPost]
        public async Task<ActionResult<Empleado>> Registrar(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();

            // Usamos "Buscar" que es el nombre real de tu método GET por ID
            return CreatedAtAction(nameof(Buscar), new { id = empleado.IdEmpleado }, empleado);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Editar(int id, Empleado empleado)
        {
            if (id != empleado.IdEmpleado) return BadRequest();

            // Re-activación automática al actualizar datos
            empleado.Estado = true;

            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            // Soft Delete: Inactivo en lugar de eliminar
            empleado.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Empleado marcado como inactivo" });
        }

        [HttpPatch("estado/{id}")]
        public async Task<ActionResult> CambiarEstado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null) return NotFound();

            empleado.Estado = !empleado.Estado;
            await _context.SaveChangesAsync();

            return Ok(empleado);
        }
    }
}