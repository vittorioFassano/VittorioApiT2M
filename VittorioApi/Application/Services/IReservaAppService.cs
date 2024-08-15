using System.Collections.Generic;
using System.Threading.Tasks;
using VittorioApiT2M.Application.DTOs;

namespace VittorioApiT2M.Application.Services
{
    public interface IReservaAppService
    {
        Task<IEnumerable<ReservaDto>> ObterTodasReservas();
        Task<ReservaDto?> ObterReservaPorId(int id);
        Task AdicionarReserva(ReservaDto reservaDto);
        Task AtualizarReserva(ReservaDto reservaDto);
        Task RemoverReserva(int id);
        Task ConfirmarReserva(int id);
    }
}
