using System.Data;

namespace Project2Api.DbTools
{
    public interface IDbClient
    {
        Task<DataTable> ExecuteQueryAsync(string query);

        Task<int> ExecuteNonQueryAsync(string query);
    }
}
