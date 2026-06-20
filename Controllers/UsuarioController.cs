using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiMovil.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return usuario;
        }

        [HttpPost("activar")]
        public async Task<IActionResult> ActivarUsuario([FromBody] RegistrarUsuarioDto dto)
        {
            var empleadoExiste = await _context.Empleados.AnyAsync(e => e.IdEmpleado == dto.IdEmpleado);
            if (!empleadoExiste) return NotFound("El empleado no existe.");

            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.IdEmpleado == dto.IdEmpleado);
            if (usuarioExiste) return BadRequest("Este empleado ya tiene un usuario activo.");

            var nuevoUsuario = new Usuario
            {
                IdEmpleado = dto.IdEmpleado,
                UsuarioNombre = dto.UsuarioNombre,
                Clave = dto.Clave,
                Rol = dto.Rol
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            return Ok("Usuario activado con éxito.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuario)
        {
            if (id != usuario.IdUsuario) return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.Estado = false;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario desactivado correctamente" });
        }
    }

    public class RegistrarUsuarioDto
    {
        public int IdEmpleado { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}