using System.Data;

namespace Project2Api.IDbClient
{
    public interface IDbClient
    {
        Task<DataTable> ExecuteQueryAsync(string query);

        Task<int> ExecuteNonQueryAsync(string query);
    }
}
