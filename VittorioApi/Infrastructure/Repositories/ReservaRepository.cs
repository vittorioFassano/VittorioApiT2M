using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using VittorioApiT2M.Application.Data;
using VittorioApiT2M.Application.DTOs;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;

namespace VittorioApiT2M.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        private readonly IDbConnection _dbConnection;

        public ReservaRepository(IDbConnection dbConnection, IDapperWrapper dapperWrapper)
        {
            _dbConnection = dbConnection;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<Reservas?> ObterPorId(int id)
        {
            try
            {
                var query = "SELECT * FROM Reservas WHERE Id = @Id";
                return await _dapperWrapper.QuerySingleOrDefaultAsync<Reservas>(_dbConnection, query, new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Reservas>> ObterTodas()
        {
            try
            {
                var query = "SELECT * FROM Reservas";
                return await _dapperWrapper.QueryAsync<Reservas>(_dbConnection, query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
        }

        public async Task Adicionar(Reservas reserva)
        {
            if (reserva == null)
            {
                throw new ArgumentNullException(nameof(reserva), "Reserva não pode ser nula.");
            }

            var query = @"
            INSERT INTO Reservas (NomeCliente, EmailCliente, DataReserva, HoraReserva, NumeroPessoas, Confirmada)
            VALUES (@NomeCliente, @EmailCliente, @DataReserva, @HoraReserva, @NumeroPessoas, @Confirmada)";

            var parametros = new
            {
                NomeCliente = reserva.NomeCliente,
                EmailCliente = reserva.EmailCliente,
                DataReserva = reserva.DataReserva,
                HoraReserva = reserva.HoraReserva,
                NumeroPessoas = reserva.NumeroPessoas,
                Confirmada = reserva.Confirmada
            };

            await _dbConnection.ExecuteAsync(query, parametros);
        }

        public async Task Atualizar(Reservas reserva)
        {
            if (reserva == null)
            {
                throw new ArgumentNullException(nameof(reserva), "Reserva não pode ser nula.");
            }

            try
            {
                var query = @"
            UPDATE Reservas
            SET NomeCliente = @NomeCliente,
                EmailCliente = @EmailCliente,
                DataReserva = @DataReserva,
                HoraReserva = @HoraReserva,
                NumeroPessoas = @NumeroPessoas,
                Confirmada = @Confirmada
            WHERE Id = @Id";

                var parametros = new
                {
                    Id = reserva.Id,
                    NomeCliente = reserva.NomeCliente,
                    EmailCliente = reserva.EmailCliente,
                    DataReserva = reserva.DataReserva,
                    HoraReserva = reserva.HoraReserva,
                    NumeroPessoas = reserva.NumeroPessoas,
                    Confirmada = reserva.Confirmada
                };

                await _dapperWrapper.ExecuteAsync(_dbConnection, query, parametros);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar reserva no banco de dados: {ex.Message}");
                throw;
            }
        }


        public async Task Remover(int id)
        {
            try
            {
                var query = "DELETE FROM Reservas WHERE Id = @Id";
                await _dapperWrapper.ExecuteAsync(_dbConnection, query, new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao remover reserva do banco de dados: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Reservas>> ObterReservasPorNomeEmailESemana(string nomeCliente, string emailCliente, DateTime inicioSemana, DateTime fimSemana)
        {
            if (inicioSemana > fimSemana)
            {
                throw new ArgumentException("A data de início da semana não pode ser posterior à data de fim da semana.");
            }

            try
            {
                var query = @"
                SELECT * 
                FROM Reservas 
                WHERE NomeCliente = @NomeCliente 
                AND EmailCliente = @EmailCliente 
                AND DataReserva >= @InicioSemana 
                AND DataReserva <= @FimSemana";

                return await _dapperWrapper.QueryAsync<Reservas>(_dbConnection, query, new { NomeCliente = nomeCliente, EmailCliente = emailCliente, InicioSemana = inicioSemana.Date, FimSemana = fimSemana.Date });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Reservas>> ObterReservasPorEmailClienteEData(string emailCliente, DateTime dataInicial, DateTime dataFinal)
        {
            if (dataInicial > dataFinal)
            {
                throw new ArgumentException("A data inicial não pode ser posterior à data final.");
            }

            try
            {
                var query = @"
                SELECT * 
                FROM Reservas 
                WHERE EmailCliente = @EmailCliente 
                AND DataReserva >= @DataInicial 
                AND DataReserva <= @DataFinal";

                return await _dapperWrapper.QueryAsync<Reservas>(_dbConnection, query, new { EmailCliente = emailCliente, DataInicial = dataInicial.Date, DataFinal = dataFinal.Date });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
        }

    }
}
