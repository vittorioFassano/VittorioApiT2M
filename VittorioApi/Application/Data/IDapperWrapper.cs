using VittorioApiT2M.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace VittorioApiT2M.Application.Data
{
    using System.Data;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDapperWrapper
    {
        Task<T> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null);
    }

}