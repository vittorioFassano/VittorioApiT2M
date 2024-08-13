using VittorioApiT2M.Application.DTOs;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;

namespace VittorioApiT2M.Application.Services
{
    public class ReservaAppService
    {
        private readonly IReservaRepository _reservaRepository;

        public ReservaAppService(IReservaRepository reservaRepository)
        {
            _reservaRepository = reservaRepository;
        }

        public async Task<IEnumerable<ReservaDto>> ObterTodasReservas()
        {
            var reservas = await _reservaRepository.ObterTodas();
            return reservas.Select(r => new ReservaDto
            {
                Id = r.Id,
                ClienteId = r.ClienteId,
                DataReserva = r.DataReserva,
                HoraReserva = r.HoraReserva,
                NumeroPessoas = r.NumeroPessoas,
                Confirmada = r.Confirmada
            });
        }

        public async Task AdicionarReserva(ReservaDto reservaDto)
        {
            var reserva = new Reservas
            {
                ClienteId = reservaDto.ClienteId,
                DataReserva = reservaDto.DataReserva,
                HoraReserva = reservaDto.HoraReserva,
                NumeroPessoas = reservaDto.NumeroPessoas,
                Confirmada = reservaDto.Confirmada
            };

            await _reservaRepository.Adicionar(reserva);
        }
    }
}
