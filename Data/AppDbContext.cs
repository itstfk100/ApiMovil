using Microsoft.EntityFrameworkCore;
using ApiMovil.Models;

namespace ApiMovil.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options
        ) : base(options)
        {

        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Planificacion> Planificaciones { get; set; }
        public DbSet<Marcacion> Marcaciones { get; set; }

    }
}
