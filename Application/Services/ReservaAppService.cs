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

        public async Task AtualizarReserva(ReservaDto reservaDto)
        {
            try
            {
                await ValidarReserva(reservaDto);

                var reservaExistente = await _reservaRepository.ObterPorId(reservaDto.Id);
                if (reservaExistente == null)
                {
                    Console.WriteLine($"Reserva com ID {reservaDto.Id} não encontrada.");
                    throw new InvalidOperationException($"Reserva com ID {reservaDto.Id} não encontrada.");
                }

                reservaExistente.ClienteId = reservaDto.ClienteId;
                reservaExistente.DataReserva = reservaDto.DataReserva;
                reservaExistente.HoraReserva = reservaDto.HoraReserva;
                reservaExistente.NumeroPessoas = reservaDto.NumeroPessoas;
                reservaExistente.Confirmada = reservaDto.Confirmada;

                await _reservaRepository.Atualizar(reservaExistente);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar reserva: {ex.Message}");
                throw new ApplicationException("Erro ao atualizar reserva.", ex);
            }
        }

        public async Task AdicionarReserva(ReservaDto reservaDto)
        {
            try
            {
                await ValidarReserva(reservaDto);

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

        public async Task ConfirmarReserva(int id)
        {
            try
            {
                var reserva = await _reservaRepository.ObterPorId(id);
                if (reserva == null)
                {
                    throw new ApplicationException($"Reserva com ID {id} não encontrada.");
                }

                // Verificar se a confirmação é válida (até 24 horas antes do horário marcado)
                if (DateTime.UtcNow > reserva.DataReserva.Add(reserva.HoraReserva) - TimeSpan.FromHours(24))
                {
                    throw new ApplicationException("Sua reserva foi CANCELADA, pois não houve confirmação em até 24 horas antes!");
                }

                reserva.Confirmada = true;
                await _reservaRepository.Atualizar(reserva);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao confirmar reserva com ID {id}: {ex.Message}");
                throw new ApplicationException($"Erro ao confirmar reserva com ID {id}.", ex);
            }
        }

        private async Task ValidarReserva(ReservaDto reservaDto)
        {
            var agora = DateTime.UtcNow;

            var dataHoraReserva = reservaDto.DataReserva.Date.Add(reservaDto.HoraReserva);

            // Verificar se a data da reserva sé futura
            if (dataHoraReserva <= agora)
            {
                throw new ApplicationException("A reserva deve ser para uma data futura!");
            }

            // Verificar se o horário está dentro do intervalo permitido (11h - 23h)
            if (reservaDto.HoraReserva < new TimeSpan(11, 0, 0) || reservaDto.HoraReserva > new TimeSpan(23, 59, 0))
            {
                throw new ApplicationException("O horário da reserva deve estar entre 11h e 23h!");
            }

            // Verificar se é um dia da semana válido (terça a domingo)
            var diaDaSemana = reservaDto.DataReserva.DayOfWeek;
            if (diaDaSemana == DayOfWeek.Monday)
            {
                throw new ApplicationException("As reservas são permitidas somente de terça a domingo!");
            }

            // Verificar se o usuário já tem uma reserva no mesmo dia
            var reservasExistentes = await _reservaRepository.ObterReservasPorClienteIdESemana(
                reservaDto.ClienteId,
                reservaDto.DataReserva.Date,
                reservaDto.DataReserva.Date.AddDays(6)
            );

            if (reservasExistentes.Any())
            {
                throw new ApplicationException("Você já possui uma reserva para este dia!");
            }
        }

    }
}
