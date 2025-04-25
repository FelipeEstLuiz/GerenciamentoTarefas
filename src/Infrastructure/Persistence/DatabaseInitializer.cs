using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        try
        {
            string targetDb = GetDatabaseNameFromConnectionString(connectionString);
            string masterConnectionString = ReplaceDatabaseInConnectionString(connectionString, "master");

            using (SqlConnection masterConnection = new(masterConnectionString))
            {
                await masterConnection.OpenAsync();

                string createDbSql = $@"
                    IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{targetDb}')
                        CREATE DATABASE [{targetDb}]
                ";

                await masterConnection.ExecuteAsync(createDbSql);
            }

            using SqlConnection targetConnection = new(connectionString);
            await targetConnection.OpenAsync();

            string script = SqlScriptLoader.LoadInitSql();
            await targetConnection.ExecuteAsync(script);
        }
        catch { }
    }

    private static string GetDatabaseNameFromConnectionString(string conn)
        => new SqlConnectionStringBuilder(conn).InitialCatalog;

    private static string ReplaceDatabaseInConnectionString(string conn, string newDb)
    {
        SqlConnectionStringBuilder builder = new(conn)
        {
            InitialCatalog = newDb
        };
        return builder.ToString();
    }
}