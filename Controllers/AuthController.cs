using ApiMovil.Data;
using ApiMovil.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMovil.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request
        )
        {
            // Log para debug (ver en la consola del backend)
            Console.WriteLine($"Intento de login - Usuario: '{request.Usuario}', Clave: '{request.Clave}'");

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.UsuarioNombre.Trim().ToLower() == request.Usuario.Trim().ToLower() &&
                    u.Clave.Trim() == request.Clave.Trim() &&
                    u.Estado == true
                );

            if (usuario == null)
            {
                // Log de fallo
                Console.WriteLine("Login fallido: Usuario no encontrado, clave errónea o inactivo.");
                return Unauthorized(
                    new { mensaje = "Usuario o contraseña incorrectos" }
                );
            }

            Console.WriteLine($"Login exitoso: {usuario.UsuarioNombre}");
            return Ok(new
            {
                usuario.IdUsuario,
                usuario.UsuarioNombre,
                usuario.Rol
            });
        }
    }
}