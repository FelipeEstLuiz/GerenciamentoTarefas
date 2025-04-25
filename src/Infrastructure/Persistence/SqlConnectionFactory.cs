using Domain.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.Persistence;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default")
            ?? throw new ArgumentNullException(nameof(_connectionString), "Connection string 'Default' not found.");
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
