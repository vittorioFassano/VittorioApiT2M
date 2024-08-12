using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;

namespace VittorioApiT2M.Domain.Services
{
    public class ReservaService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaService(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task ConfirmarReserva(int id)
        {
            var reserva = await _reservaRepository.ObterPorId(id);
            if (reserva != null)
            {
                reserva.Confirmada = true;
                await _reservaRepository.Atualizar(reserva);
            }
        }
    }
}
