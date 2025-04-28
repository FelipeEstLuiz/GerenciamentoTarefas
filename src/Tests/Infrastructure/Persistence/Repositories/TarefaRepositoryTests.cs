using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tests.Infrastructure.Persistence.Fixtures;

namespace Tests.Infrastructure.Persistence.Repositories;

public class TarefaRepositoryTests : IClassFixture<SqlServerTestFixture>, IAsyncLifetime
{
    private readonly SqlServerTestFixture _fixture;
    private readonly ILogger<TarefaRepository> _loggerMock;
    private readonly TarefaRepository _tarefaRepository;

    public TarefaRepositoryTests(SqlServerTestFixture fixture)
    {
        _fixture = fixture;

        SqlConnectionFactory dbConnection = new(_fixture.Configuration);

        _loggerMock = Substitute.For<ILogger<TarefaRepository>>();

        _tarefaRepository = new TarefaRepository(dbConnection, _loggerMock);
    }

    public async Task InitializeAsync() => await _fixture.ResetDatabaseAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AddAsync_DeveRetornarSucesso_QuandoInsercaoForBemSucedida()
    {
        Tarefa tarefa = CreateTarefa();

        tarefa = await _tarefaRepository.AddAsync(tarefa, CancellationToken.None);

        Assert.True(tarefa.Id > 0);
    }

    [Fact]
    public async Task AddAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None));

        Assert.Equal("Erro ao inserir tarefa", exception.Message);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarSucesso_QuandoEncontrar()
    {
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        Tarefa? tarefaResponse = await _tarefaRepository.GetByIdAsync(tarefa.Id, CancellationToken.None);

        Assert.NotNull(tarefaResponse);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarVazio_QuandoNaoEncontrar()
    {
        Tarefa? tarefaResponse = await _tarefaRepository.GetByIdAsync(10, CancellationToken.None);

        Assert.Null(tarefaResponse);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.GetByIdAsync(1, CancellationToken.None));

        Assert.Equal("Erro ao obter tarefa", exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoEncontrar()
    {
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        ServerSide<IEnumerable<Tarefa>> tarefaResponse = await _tarefaRepository.GetAllAsync(1, 5, CancellationToken.None);

        Assert.NotNull(tarefaResponse);
        Assert.Equal(2, tarefaResponse.TotalData);
        Assert.Equal(2, tarefaResponse.Data!.Count());
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoNaoEncontrar()
    {
        ServerSide<IEnumerable<Tarefa>> tarefaResponse = await _tarefaRepository.GetAllAsync(1, 5, CancellationToken.None);

        Assert.NotNull(tarefaResponse);
        Assert.Equal(0, tarefaResponse.TotalData);
        Assert.Empty(tarefaResponse.Data!);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.GetAllAsync(1,2, CancellationToken.None));

        Assert.Equal("Erro ao obter tarefas", exception.Message);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoEncontrar_Paginacao()
    {
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        ServerSide<IEnumerable<Tarefa>> tarefaResponse = await _tarefaRepository.GetAllAsync(1, 5, CancellationToken.None);

        Assert.NotNull(tarefaResponse);
        Assert.Equal(6, tarefaResponse.TotalData);
        Assert.Equal(5, tarefaResponse.Data!.Count());
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizar()
    {
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        tarefa.SetStatus(StatusTarefa.EmProgresso);

        await _tarefaRepository.UpdateAsync(tarefa, CancellationToken.None);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.UpdateAsync(CreateTarefa(), CancellationToken.None));

        Assert.Equal("Erro ao atualizar tarefa", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarTrue_QuandoRemover()
    {
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        Tarefa? tarefaResponse = await _tarefaRepository.GetByIdAsync(tarefa.Id, CancellationToken.None);

        Assert.NotNull(tarefaResponse);

        bool result = await _tarefaRepository.DeleteAsync(tarefa.Id, CancellationToken.None);

        tarefaResponse = await _tarefaRepository.GetByIdAsync(tarefa.Id, CancellationToken.None);

        Assert.Null(tarefaResponse);
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.DeleteAsync(1, CancellationToken.None));

        Assert.Equal("Erro ao remover tarefa", exception.Message);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaAsync_DeveRetornarTrue_QuandoEncontrar()
    {
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        bool result = await _tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            tarefa.Titulo,
            CancellationToken.None
        );

        Assert.True(result);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync("Titulo", CancellationToken.None));

        Assert.Equal("Erro ao validar tarefa pelo titulo", exception.Message);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaAsync_DeveRetornarFalse_QuandoNaoEncontrar()
    {
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        bool result = await _tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            "Titulo nao existe",
            CancellationToken.None
        );

        Assert.False(result);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync_DeveRetornarTrue_QuandoEncontrar()
    {
        await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        bool result = await _tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            tarefa.Id,
            tarefa.Titulo,
            CancellationToken.None
        );

        Assert.True(result);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync_DeveRetornarException_QuandoFalhar()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
           .AddInMemoryCollection(new Dictionary<string, string?>
           {
                {
                    "ConnectionStrings:DefaultConnection",
                    "Server=localhost,9999;Database=FakeDb;User Id=sa;Password=WrongPassword;"
                }
           })
           .Build();

        SqlConnectionFactory fakeDbConnection = new(configuration);
        TarefaRepository tarefaRepository = new(fakeDbConnection, _loggerMock);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () => await tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(1, "Titulo", CancellationToken.None));

        Assert.Equal("Erro ao validar tarefa pelo titulo", exception.Message);
    }

    [Fact]
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync_DeveRetornarFalse_QuandoNaoEncontrar()
    {
        Tarefa tarefa = await _tarefaRepository.AddAsync(CreateTarefa(), CancellationToken.None);

        bool result = await _tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            tarefa.Id,
            "Titulo nao existe",
            CancellationToken.None
        );

        Assert.False(result);
    }

    private static Tarefa CreateTarefa() => new(
        "Título da Tarefa",
        "Descrição da Tarefa"
    );
}
