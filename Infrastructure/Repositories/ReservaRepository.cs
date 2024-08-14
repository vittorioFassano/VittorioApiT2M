using Dapper;
using Microsoft.Data.SqlClient;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;
using System.Data;
using System;

namespace VittorioApiT2M.Infrastructure.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly IDbConnection _dbConnection;

        public ReservaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Reservas?> ObterPorId(int id)
        {
            try
            {
                var query = "SELECT * FROM Reservas WHERE Id = @Id";
                return await _dbConnection.QuerySingleOrDefaultAsync<Reservas>(query, new { Id = id });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Reservas>> ObterTodas()
        {
            try
            {
                var query = "SELECT * FROM Reservas";
                return await _dbConnection.QueryAsync<Reservas>(query);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw;
            }
        }

        public async Task Adicionar(Reservas reserva)
        {
            if (reserva == null)
            {
                throw new ArgumentNullException(nameof(reserva), "Reserva não pode ser nula.");
            }

            try
            {
                var query = "INSERT INTO Reservas (ClienteId, DataReserva, HoraReserva, NumeroPessoas, Confirmada) VALUES (@ClienteId, @DataReserva, @HoraReserva, @NumeroPessoas, @Confirmada)";
                await _dbConnection.ExecuteAsync(query, reserva);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao adicionar reserva no banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao adicionar reserva: {ex.Message}");
                throw;
            }
        }

        public async Task Atualizar(Reservas reserva)
        {
            if (reserva == null)
            {
                throw new ArgumentNullException(nameof(reserva), "Reserva não pode ser nula.");
            }

            try
            {
                var query = "UPDATE Reservas SET ClienteId = @ClienteId, DataReserva = @DataReserva, HoraReserva = @HoraReserva, NumeroPessoas = @NumeroPessoas, Confirmada = @Confirmada WHERE Id = @Id";
                await _dbConnection.ExecuteAsync(query, reserva);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao atualizar reserva no banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao atualizar reserva: {ex.Message}");
                throw;
            }
        }

        public async Task Remover(int id)
        {
            try
            {
                var query = "DELETE FROM Reservas WHERE Id = @Id";
                await _dbConnection.ExecuteAsync(query, new { Id = id });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao remover reserva do banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao remover reserva: {ex.Message}");
                throw;
            }
        }
        public async Task<IEnumerable<Reservas>> ObterReservasPorClienteIdESemana(int clienteId, DateTime inicioSemana, DateTime fimSemana)
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
            WHERE ClienteId = @ClienteId 
            AND DataReserva >= @InicioSemana 
            AND DataReserva <= @FimSemana";

                return await _dbConnection.QueryAsync<Reservas>(query, new { ClienteId = clienteId, InicioSemana = inicioSemana.Date, FimSemana = fimSemana.Date });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao acessar o banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
                throw;
            }
        }

    }
}
