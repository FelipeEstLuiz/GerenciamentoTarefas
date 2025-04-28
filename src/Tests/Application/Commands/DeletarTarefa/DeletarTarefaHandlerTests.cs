using Application.Commands.DeletarTarefa;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using NSubstitute;

namespace Tests.Application.Commands.DeletarTarefa;

public class DeletarTarefaHandlerTests
{
    [Fact]
    public async Task DeletarTarefaHandler_Sucesso()
    {
        DeletarTarefaCommand command = new(1);

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        IObterTarefaService obterTarefaService = Substitute.For<IObterTarefaService>();

        Tarefa tarefa = new(
            command.Id,
            "Titulo",
            "Descricao",
            StatusTarefa.Pendente,
            DateTime.UtcNow,
            null
        );

        obterTarefaService
            .ObterTarefaPorIdOrThrow(command.Id, Arg.Any<CancellationToken>())
            .Returns(tarefa);

        tarefaRepository
            .DeleteAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);

        DeletarTarefaHandler handler = new(tarefaRepository, obterTarefaService);

        string result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Tarefa removida com sucesso", result);
    }

    [Fact]
    public async Task DeletarTarefaHandler_LancarExceptionStatusNaoPendente()
    {
        DeletarTarefaCommand command = new(1);

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        IObterTarefaService obterTarefaService = Substitute.For<IObterTarefaService>();

        Tarefa tarefa = new(
            command.Id,
            "Titulo",
            "Descricao",
            StatusTarefa.EmProgresso,
            DateTime.UtcNow,
            null
        );

        obterTarefaService
            .ObterTarefaPorIdOrThrow(command.Id, Arg.Any<CancellationToken>())
            .Returns(tarefa);

        DeletarTarefaHandler handler = new(tarefaRepository, obterTarefaService);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () 
            => await handler.Handle(command, CancellationToken.None));

        Assert.Equal("A tarefa não pode ser removida pois não está pendente", exception.Message);
    }
}
