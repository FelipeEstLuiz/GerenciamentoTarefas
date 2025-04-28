using Application.DTOs;
using Application.Queries.ObterTarefaPorId;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using NSubstitute;

namespace Tests.Application.Queries.ObterTarefaPorId;

public class ObterTarefaPorIdHandlerTests
{
    [Fact]
    public async Task ObterTarefaPorIdHandler_RetornarSucesso()
    {
        ObterTarefaPorIdQuery command = new(1);

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

        ObterTarefaPorIdHandler handler = new(obterTarefaService);

        TarefaDto result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(tarefa.Descricao, result.Descricao);
        Assert.Equal(tarefa.Titulo, result.Titulo);
        Assert.Equal((int)tarefa.CodigoStatusTarefa, result.CodigoStatusTarefa);
        Assert.Equal(tarefa.DataCriacao, result.DataCriacao);
        Assert.Equal(tarefa.DataConclusao, result.DataConclusao);
        Assert.Null(result.DataConclusao);
    }
}
