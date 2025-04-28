using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class TarefaRepository(IDbConnectionFactory dbConnection, ILogger<TarefaRepository> logger) : ITarefaRepository
{
    public async Task<Tarefa?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                Id,
                Titulo,
                Descricao,
                CodigoStatusTarefa,
                DataCriacao,
                DataConclusao
            FROM Tarefa
            WHERE Id = @Id
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<Tarefa?>(new CommandDefinition(
                sql,
                new { Id = id },
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter tarefa ({id}): {Message}", id, ex.Message);
            throw new ValidacaoException("Erro ao obter tarefa");
        }
    }

    public async Task<ServerSide<IEnumerable<Tarefa>>> GetAllAsync(
        int page,
        int limit,
        CancellationToken cancellationToken
    )
    {
        const string sql = @"
                SELECT 
                    Id,
                    Titulo,
                    Descricao,
                    CodigoStatusTarefa,
                    DataCriacao,
                    DataConclusao
                FROM Tarefa
                ORDER BY Id DESC
                OFFSET @Offset ROWS
                FETCH NEXT @Limit ROWS ONLY;

                SELECT COUNT(1) FROM Tarefa;
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            using SqlMapper.GridReader multi = await conn.QueryMultipleAsync(new CommandDefinition(
                sql,
                new
                {
                    Offset = page == 0 ? page : page - 1,
                    Limit = limit
                },
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ));

            ServerSide<IEnumerable<Tarefa>> serverSide = new(
                await multi.ReadAsync<Tarefa>(),
                await multi.ReadSingleAsync<int>()
            );
            return serverSide;

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao obter tarefas: {Message}", ex.Message);
            throw new ValidacaoException("Erro ao obter tarefas");
        }
    }

    public async Task<Tarefa> AddAsync(Tarefa tarefa, CancellationToken cancellationToken)
    {
        const string sql = @"
            INSERT INTO Tarefa (Titulo, Descricao)                                
            OUTPUT INSERTED.Id, INSERTED.CodigoStatusTarefa, INSERTED.DataCriacao
            VALUES (@Titulo, @Descricao)
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            dynamic response = await conn.QuerySingleAsync<dynamic>(new CommandDefinition(
                sql,
                tarefa,
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ));

            tarefa.SetId(response.Id);
            tarefa.SetStatus((StatusTarefa)response.CodigoStatusTarefa);
            tarefa.SetDataCriacao((DateTime)response.DataCriacao);

            return tarefa;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inserir tarefa: {Message}", ex.Message);
            throw new ValidacaoException("Erro ao inserir tarefa");
        }
    }

    public async Task<Tarefa> UpdateAsync(Tarefa tarefa, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Tarefa
            SET Titulo = @Titulo,
                Descricao = @Descricao,
                CodigoStatusTarefa = @Status,
                DataConclusao = @DataConclusao
            WHERE Id = @Id
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            await conn.ExecuteAsync(new CommandDefinition(
                sql,
                new
                {
                    tarefa.Id,
                    tarefa.DataConclusao,
                    Status = (int)tarefa.CodigoStatusTarefa,
                    tarefa.Titulo,
                    tarefa.Descricao
                },
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ));

            return tarefa;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar tarefa ({Id}): {Message}", tarefa.Id, ex.Message);
            throw new ValidacaoException("Erro ao atualizar tarefa");
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        const string sql = @"
            DELETE FROM Tarefa WHERE Id = @Id
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            return (await conn.ExecuteAsync(new CommandDefinition(
                 sql,
                 new
                 {
                     Id = id
                 },
                 commandType: CommandType.Text,
                 commandTimeout: 0,
                 cancellationToken: cancellationToken
             ))) > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao remover tarefa ({id}): {Message}", id, ex.Message);
            throw new ValidacaoException("Erro ao remover tarefa");
        }
    }

    public async Task<bool> ExisteTarefaComMesmoTituloNaoConcluidaAsync(string titulo, CancellationToken cancellationToken)
    {
        const string sql = @"

           SELECT CASE WHEN EXISTS (
                SELECT 1
                FROM Tarefa
                WHERE Titulo = @Titulo
                AND CodigoStatusTarefa != @Status
            ) 
            THEN 1 
            ELSE 0 
            END
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            return (await conn.QueryFirstAsync<int>(new CommandDefinition(
                sql,
                new
                {
                    Titulo = titulo,
                    Status = (int)StatusTarefa.Concluida
                },
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ))) > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao validar tarefa pelo titulo ({titulo}): {Message}", titulo, ex.Message);
            throw new ValidacaoException("Erro ao validar tarefa pelo titulo");
        }
    }

    public async Task<bool> ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
        int id,
        string titulo,
        CancellationToken cancellationToken
    )
    {
        const string sql = @"

           SELECT CASE WHEN EXISTS (
                SELECT 1
                FROM Tarefa
                WHERE Titulo = @Titulo
                AND CodigoStatusTarefa != 2
                AND Id != @Id
            ) 
            THEN 1 
            ELSE 0 
            END
        ";

        try
        {
            using IDbConnection conn = dbConnection.CreateConnection();

            return (await conn.QueryFirstAsync<int>(new CommandDefinition(
                sql,
                new
                {
                    Titulo = titulo,
                    Id = id
                },
                commandType: CommandType.Text,
                commandTimeout: 0,
                cancellationToken: cancellationToken
            ))) > 0;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao validar tarefa({id}) pelo titulo ({titulo}): {Message}", id, titulo, ex.Message);
            throw new ValidacaoException("Erro ao validar tarefa pelo titulo");
        }
    }
}
