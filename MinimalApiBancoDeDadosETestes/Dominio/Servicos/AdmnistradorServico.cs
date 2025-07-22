using MinimalApiBancoDeDadosETestes.Dominio.DTOs;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;
using MinimalApiBancoDeDadosETestes.Dominio.Interfaces;
using MinimalApiBancoDeDadosETestes.Infraestrutura.Db;

namespace MinimalApiBancoDeDadosETestes.Dominio.Servicos
{
    public class AdmnistradorServico : IAdministradorServico
    {
        private readonly DbContexto _contexto;

        public AdmnistradorServico(DbContexto contexto)
        {
            _contexto = contexto;
        }

        public Administrador? BuscaPorId(int id)
        {
            return _contexto.Admnistradores.Where(a => a.Id == id).FirstOrDefault();
        }

        public Administrador Incluir(Administrador administrador)
        {
            _contexto.Admnistradores.Add(administrador);
            _contexto.SaveChanges();

            return administrador;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Admnistradores.
                 Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();

            return adm;
                       
        }

        public List<Administrador> Todos(int? pagina)
        {
            var query = _contexto.Admnistradores.AsQueryable();

            int itensPorPagina = 10;

            if (pagina != null)
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

            return query.ToList();  
        }
       
    }
}
