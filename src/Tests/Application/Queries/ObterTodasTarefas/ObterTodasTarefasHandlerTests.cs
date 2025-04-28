using Application.DTOs;
using Application.Queries.ObterTodasTarefas;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using NSubstitute;

namespace Tests.Application.Queries.ObterTodasTarefas;

public class ObterTodasTarefasHandlerTests
{
    [Fact]
    public async Task ObterTodasTarefasHandler_RetornarSucesso()
    {
        int page = 1;
        int limit = 5;

        ObterTodasTarefasQuery command = new(page, limit);

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        List<Tarefa> tarefas = [];
        int quantidadeTarefas = 5;

        for (int i = 1; i <= quantidadeTarefas; i++)
        {
            tarefas.Add(new(
                i,
                $"Titulo {i}",
                $"Descricao {i}",
                StatusTarefa.Pendente,
                DateTime.UtcNow,
                null
            ));
        }

        ServerSide<IEnumerable<Tarefa>> retorno = new(tarefas, quantidadeTarefas);

        tarefaRepository
            .GetAllAsync(page, limit, Arg.Any<CancellationToken>())
            .Returns(retorno);

        ObterTodasTarefasHandler handler = new(tarefaRepository);

        ServerSideDto<IEnumerable<TarefaListagemDto>> result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(page, result.Page);
        Assert.Equal(limit, result.Limit);
        Assert.Equal(tarefas.Count, result.Data!.Count());
        Assert.Equal(tarefas.Count, result.TotalData);
    }

    [Fact]
    public async Task ObterTodasTarefasHandler_RetornarSucesso_Limite()
    {
        int page = 1;
        int limit = 5;

        ObterTodasTarefasQuery command = new(page, limit);

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        List<Tarefa> tarefas = [];
        int quantidadeTarefas = 10;

        for (int i = 1; i <= quantidadeTarefas; i++)
        {
            tarefas.Add(new(
                i,
                $"Titulo {i}",
                $"Descricao {i}",
                StatusTarefa.Pendente,
                DateTime.UtcNow,
                null
            ));
        }

        ServerSide<IEnumerable<Tarefa>> retorno = new(tarefas.Take(limit), quantidadeTarefas);

        tarefaRepository
            .GetAllAsync(page, limit, Arg.Any<CancellationToken>())
            .Returns(retorno);

        ObterTodasTarefasHandler handler = new(tarefaRepository);

        ServerSideDto<IEnumerable<TarefaListagemDto>> result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(page, result.Page);
        Assert.Equal(limit, result.Limit);
        Assert.Equal(limit, result.Data!.Count());
        Assert.Equal(tarefas.Count, result.TotalData);
    }

    [Fact]
    public async Task ObterTodasTarefasHandler_RetornarSucesso_NaoEncontrar()
    {
        int page = 1;
        int limit = 5;

        ObterTodasTarefasQuery command = new(page, limit);

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        List<Tarefa> tarefas = [];

        ServerSide<IEnumerable<Tarefa>> retorno = new(tarefas, tarefas.Count);

        tarefaRepository
            .GetAllAsync(page, limit, Arg.Any<CancellationToken>())
            .Returns(retorno);

        ObterTodasTarefasHandler handler = new(tarefaRepository);

        ServerSideDto<IEnumerable<TarefaListagemDto>> result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(page, result.Page);
        Assert.Equal(limit, result.Limit);
        Assert.Empty(result.Data!);
        Assert.Equal(tarefas.Count, result.TotalData);
    }
}
