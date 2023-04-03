using Npgsql;
using System.Data;

namespace Project2Api.DbTools
{
    public class DbClient : IDbClient
    {
        public readonly string _connectionString;
        
        public DbClient(IConfiguration config)
        {
            string _host = config.GetValue<string>("PostgreSQL:Host");
            string _database = config.GetValue<string>("PostgreSQL:Database");
            string _username = config.GetValue<string>("PostgreSQL:Username");
            string _password = config.GetValue<string>("PostgreSQL:Password");

            _connectionString = $"Host={_host};Database={_database};Username={_username};Password={_password};";
        }

        public async Task<DataTable> ExecuteQueryAsync(string query)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);

            try 
            {
                using var reader = await command.ExecuteReaderAsync();

                var table = new DataTable();
                table.Load(reader);
                connection.Close(); 

                return table;
            } catch (Npgsql.PostgresException e) {
                Console.Out.WriteLine(e);

                connection.Close(); 
                return new DataTable();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new NpgsqlCommand(query, connection);
             
            try
            {
                var rowsAffected = await command.ExecuteNonQueryAsync();
                
                connection.Close();
                return rowsAffected; 
            } catch (Npgsql.PostgresException e) {
                Console.Out.WriteLine($"Error while processing query '{query}': {e}");

                connection.Close(); 
                return -1;
            }
        }
    }
}
