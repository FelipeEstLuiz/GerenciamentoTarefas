using Application.Commands.CriarTarefa;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using NSubstitute;

namespace Tests.Application.Commands.CriarTarefa;

public class CriarTarefaHandlerTests
{
    [Fact]
    public async Task CriarTarefaHandler_Valido()
    {
        CriarTarefaCommand command = new()
        {
            Descricao = "Descricao",
            Titulo = "Titulo"
        };

        int id = 1;

        Tarefa tarefa = new(
            id,
            command.Titulo!,
            command.Descricao,
            StatusTarefa.EmProgresso,
            DateTime.UtcNow,
            null
        );

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        IValidarTituloTarefaService validarTituloTarefaService = Substitute.For<IValidarTituloTarefaService>();

        validarTituloTarefaService.When(
            x => x.NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync(
                command.Titulo!,
                Arg.Any<CancellationToken>()
            )
        ).Do(x => { });

        tarefaRepository
            .AddAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        CriarTarefaHandler handler = new(tarefaRepository, validarTituloTarefaService);

        TarefaDto result = await handler.Handle(command, CancellationToken.None);
        
        Assert.NotNull(result);
        Assert.Equal("Titulo", result.Titulo);
        Assert.Equal("Descricao", result.Descricao);
        Assert.Equal(id, tarefa.Id);
    }
}
