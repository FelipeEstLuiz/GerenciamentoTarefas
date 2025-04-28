using Application.Commands.AtualizarTarefa;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using NSubstitute;

namespace Tests.Application.Commands.AtualizarTarefa;

public class AtualizarTarefaHandlerTests
{
    [Fact]
    public async Task AtualizarTarefaHandler_Sucesso()
    {        
        AtualizarTarefaCommand command = new()
        {
            Id = 1,
            Titulo = "Novo Titulo",
            Descricao = "Nova Descricao",
            StatusTarefa = StatusTarefa.Concluida,
            DataConclusao = DateTime.UtcNow.AddMinutes(1)
        };

        Tarefa tarefa = new(
            command.Id.Value,
            command.Titulo!,
            command.Descricao,
            StatusTarefa.EmProgresso,
            DateTime.UtcNow,
            null
        );

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        IObterTarefaService obterTarefaService = Substitute.For<IObterTarefaService>();
        IValidarTituloTarefaService validarTituloTarefaService = Substitute.For<IValidarTituloTarefaService>();

        validarTituloTarefaService.When(
            x => x.NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync(
                command.Id.Value,
                command.Titulo!,
                Arg.Any<CancellationToken>()
            )
        ).Do(x => { });

        obterTarefaService
            .ObterTarefaPorIdOrThrow(command.Id.Value, Arg.Any<CancellationToken>())
            .Returns(tarefa);

        tarefaRepository
            .UpdateAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        AtualizarTarefaHandler handler = new(tarefaRepository, obterTarefaService, validarTituloTarefaService);

        TarefaDto result = await handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal(command.Titulo, result.Titulo);
        Assert.Equal(command.Descricao, result.Descricao);
        Assert.Equal((int)command.StatusTarefa, result.CodigoStatusTarefa);
        Assert.Equal(command.DataConclusao, result.DataConclusao);
    }
}
