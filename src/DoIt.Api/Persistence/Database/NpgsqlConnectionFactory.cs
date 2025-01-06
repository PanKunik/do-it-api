
using Npgsql;
using System.Data;

namespace DoIt.Api.Persistence.Database
{
    public class NpgsqlConnectionFactory
        : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public NpgsqlConnectionFactory(
            string connectionString
        )
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            return connection;
        }
    }
}
