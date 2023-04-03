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
        /// Executes query and returns number of affected rows
        /// </summary>
        Task<int> ExecuteNonQueryAsync(string query);
    }
}
