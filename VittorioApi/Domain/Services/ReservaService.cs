using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VittorioApiT2M.Application.DTOs;

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
            try
            {
                var reserva = await _reservaRepository.ObterPorId(id);
                if (reserva == null)
                {
                    Console.WriteLine($"Reserva com ID {id} não encontrada.");
                    throw new InvalidOperationException($"Reserva com ID {id} não encontrada.");
                }

                reserva.Confirmada = true;
                await _reservaRepository.Atualizar(reserva);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao confirmar reserva: {ex.Message}");
                throw new ApplicationException("Ocorreu um erro ao confirmar a reserva.", ex);
            }
        }


        public async Task<IEnumerable<ReservaDto>> ObterReservasPorNomeEmailESemana(string nomeCliente, string emailCliente, DateTime inicioSemana, DateTime fimSemana)
        {
            var reservas = await _reservaRepository.ObterReservasPorNomeEmailESemana(nomeCliente, emailCliente, inicioSemana, fimSemana);
            return reservas.Select(r => new ReservaDto
            {
                Id = r.Id,
                NomeCliente = r.NomeCliente,
                EmailCliente = r.EmailCliente,
                DataReserva = r.DataReserva,
                HoraReserva = r.HoraReserva,
                NumeroPessoas = r.NumeroPessoas,
                Confirmada = r.Confirmada
            });
        }
    }
}
