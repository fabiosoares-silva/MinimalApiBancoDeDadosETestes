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
        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Admnistradores.
                 Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();

            return adm;
                       
        }

        List<Administrador> IAdministradorServico.Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }
    }
}
