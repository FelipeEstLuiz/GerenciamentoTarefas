using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using NSubstitute;
using System.Net;

namespace Tests.Application.Services;

public class ObterTarefaServiceTests
{
    [Fact]
    public async Task ObterTarefaPorIdOrThrow_RetornarTarefa()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        tarefaRepository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(tarefa);

        ObterTarefaService service = new(tarefaRepository);

        Tarefa resultado = await service.ObterTarefaPorIdOrThrow(1, CancellationToken.None);

        Assert.NotNull(resultado);
        Assert.Equal(tarefa.Id, resultado.Id);
    }

    [Fact]
    public async Task ObterTarefaPorIdOrThrow_RetornarException()
    {
        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();
        tarefaRepository.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns((Tarefa?)null);

        ObterTarefaService service = new(tarefaRepository);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async () 
            => await service.ObterTarefaPorIdOrThrow(1, CancellationToken.None));

        Assert.Equal("Tarefa não encontrada", exception.Message);
        Assert.Equal(HttpStatusCode.NotFound, exception.HttpStatusCode);
    }
}
