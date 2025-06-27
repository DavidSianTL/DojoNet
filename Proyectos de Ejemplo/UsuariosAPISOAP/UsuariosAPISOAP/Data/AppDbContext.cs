using Microsoft.EntityFrameworkCore;
using UsuariosAPISOAP.Models;

namespace UsuariosAPISOAP.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UsuarioEF> UsuariosEF { get; set; }
        public DbSet<LogEvento> LogEventos { get; set; }
        //se agrega medicion de metricas
        public DbSet<MetricasSolicitud> Metricas { get; set; }
    }
}
