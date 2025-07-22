using MinimalApiBancoDeDadosETestes.Dominio.DTOs;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;

namespace MinimalApiBancoDeDadosETestes.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDTO);
        Administrador Incluir(Administrador administrador);
        Administrador? BuscaPorId(int id);
        List<Administrador> Todos(int? pagina);
    }
}
