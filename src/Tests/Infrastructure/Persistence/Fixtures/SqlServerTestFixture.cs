using Dapper;
using Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Tests.Infrastructure.Persistence.Fixtures;

public class SqlServerTestFixture : IAsyncLifetime
{
    public IConfiguration Configuration { get; private set; }

    public SqlServerTestFixture()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();

        Configuration = config;
    }

    public async Task InitializeAsync()
    {
        string fullConnectionString = Configuration.GetConnectionString("DefaultConnection")!;

        Match match = Regex.Match(fullConnectionString, @"Database=([^;]*)");

        string nomeDatabase = match.Groups[1].Value;

        string masterConnectionString = fullConnectionString.Replace($"Database={nomeDatabase}", "Database=master");

        using (SqlConnection connection = new(masterConnectionString))
        {
            await connection.OpenAsync();
            await connection.ExecuteAsync($@"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{nomeDatabase}')
                    CREATE DATABASE {nomeDatabase};
            ");
        }

        using (SqlConnection connection = new(fullConnectionString))
            await connection.ExecuteAsync(SqlScriptLoader.LoadInitSql());
    }

    public async Task ResetDatabaseAsync()
    {
        using SqlConnection connection = new(Configuration.GetConnectionString("DefaultConnection"));
        await connection.OpenAsync();

        await connection.ExecuteAsync(@"
            DELETE FROM Tarefa;          
        ");

        await connection.ExecuteAsync(@"
            DBCC CHECKIDENT ('Tarefa', RESEED, 0);          
        ");
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
