using VittorioApiT2M.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VittorioApiT2M.Domain.Repositories
{
    public interface IReservaRepository
    {
        Task<Reservas?> ObterPorId(int id);
        Task<IEnumerable<Reservas>> ObterTodas();
        Task Adicionar(Reservas reserva);
        Task Atualizar(Reservas reserva);
        Task Remover(int id);
        Task<IEnumerable<Reservas>> ObterReservasPorEmailClienteEData(string emailCliente, DateTime dataInicial, DateTime dataFinal);
        Task<IEnumerable<Reservas>> ObterReservasPorNomeEmailESemana(string nomeCliente, string emailCliente, DateTime inicioSemana, DateTime fimSemana);
    }
}
