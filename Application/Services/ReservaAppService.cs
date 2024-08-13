using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            try
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
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao obter todas as reservas: {ex.Message}");

                throw new ApplicationException("Erro ao obter todas as reservas.", ex);
            }
        }

        public async Task AdicionarReserva(ReservaDto reservaDto)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar reserva: {ex.Message}");
                throw new ApplicationException("Erro ao adicionar reserva.", ex);
            }
        }

        public async Task RemoverReserva(int id)
        {
            try
            {
                await _reservaRepository.Remover(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover reserva com ID {id}: {ex.Message}");
                throw new ApplicationException($"Erro ao remover reserva com ID {id}.", ex);
            }
        }

        public async Task<ReservaDto?> ObterReservaPorId(int id)
        {
            try
            {
                var reserva = await _reservaRepository.ObterPorId(id);
                if (reserva == null)
                {
                    return null;
                }

                return new ReservaDto
                {
                    Id = reserva.Id,
                    ClienteId = reserva.ClienteId,
                    DataReserva = reserva.DataReserva,
                    HoraReserva = reserva.HoraReserva,
                    NumeroPessoas = reserva.NumeroPessoas,
                    Confirmada = reserva.Confirmada
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter reserva com ID {id}: {ex.Message}");
                throw new ApplicationException($"Erro ao obter reserva com ID {id}.", ex);
            }
        }
    }
}

