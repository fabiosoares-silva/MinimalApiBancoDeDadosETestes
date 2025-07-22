using Microsoft.EntityFrameworkCore;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;

namespace MinimalApiBancoDeDadosETestes.Infraestrutura.Db
{
    public class DbContexto : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings;

        public DbContexto(IConfiguration configuracaoAppSettings)
        {
            _configuracaoAppSettings = configuracaoAppSettings;
        }

        public DbSet<Administrador> Admnistradores { get; set; } = default!;
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(new Administrador
            {
                Id = 1,
                Email = "Administrador@teste.com",
                Senha = "123456",
                Perfil = "Admin"
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("DefaultConnection")?.ToString();

                if (!string.IsNullOrEmpty(stringConexao))
                {
                    optionsBuilder.UseMySql(stringConexao,
                    ServerVersion.AutoDetect(stringConexao));
                }
            }
            
        }
    }
}
