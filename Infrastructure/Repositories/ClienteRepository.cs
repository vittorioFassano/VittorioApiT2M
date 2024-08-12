using Dapper;
using Microsoft.Data.SqlClient;
using VittorioApiT2M.Domain.Entities;
using VittorioApiT2M.Domain.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace VittorioApiT2M.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IDbConnection _dbConnection;

        public ClienteRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<Clientes?> ObterPorEmail(string email)
        {
            try
            {
                var query = "SELECT * FROM Cliente WHERE Email = @Email";
                return await _dbConnection.QuerySingleOrDefaultAsync<Clientes>(query, new { Email = email });
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

        public async Task Adicionar(Clientes cliente)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "Cliente não pode ser nulo.");
            }

            try
            {
                var query = "INSERT INTO Cliente (Nome, Email, Senha) VALUES (@Nome, @Email, @Senha)";
                await _dbConnection.ExecuteAsync(query, cliente);
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Erro ao adicionar cliente no banco de dados: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado ao adicionar cliente: {ex.Message}");
                throw;
            }
        }
    }
}
