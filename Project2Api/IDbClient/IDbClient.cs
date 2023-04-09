using System.Data;

namespace Project2Api.DbTools
{
    public interface IDbClient
    {
        /// <summary>
        /// Executes query and returns DataTable
        /// </summary>
        Task<DataTable> ExecuteQueryAsync(string query);

        /// <summary>
        /// Executes query and returns number of affected rows if successful, else -1
        /// </summary>
        Task<int> ExecuteNonQueryAsync(string query);
    }
}
