using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using VittorioApiT2M.Application.Data;

public class DapperWrapper : IDapperWrapper
{
    public async Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return await connection.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return await connection.ExecuteAsync(sql, param);
    }
}
