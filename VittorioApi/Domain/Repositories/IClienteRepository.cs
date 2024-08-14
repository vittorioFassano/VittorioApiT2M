using VittorioApiT2M.Domain.Entities;
using System.Threading.Tasks;

namespace VittorioApiT2M.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<Clientes?> ObterPorEmail(string email);
        Task Adicionar(Clientes cliente);
    }
}
