using Microsoft.EntityFrameworkCore;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;

namespace MinimalApiBancoDeDadosETestes.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        public DbSet<Administrador> Admnistradores { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}
