using MinimalApiBancoDeDadosETestes.Dominio.DTOs;
using MinimalApiBancoDeDadosETestes.Dominio.Entidades;

namespace MinimalApiBancoDeDadosETestes.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        List<Administrador> Login(LoginDTO loginDTO);
    }
}
