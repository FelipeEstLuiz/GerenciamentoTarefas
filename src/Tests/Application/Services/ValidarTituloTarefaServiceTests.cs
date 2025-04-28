using Application.Services;
using Domain.Exceptions;
using Domain.Repositories;
using NSubstitute;

namespace Tests.Application.Services;
public class ValidarTituloTarefaServiceTests
{
    [Fact]
    public async Task NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync_Valido()
    {
        int id = 1;
        string titulo = "Título da Tarefa";
        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            id,
            titulo,
            Arg.Any<CancellationToken>()
        ).Returns(false);

        ValidarTituloTarefaService service = new(tarefaRepository);

        await service.NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync(id, titulo, CancellationToken.None);

        await tarefaRepository.Received(1).ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            id,
            titulo,
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync_Valido()
    {
        string titulo = "Título da Tarefa";
        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            titulo,
            Arg.Any<CancellationToken>()
        ).Returns(false);

        ValidarTituloTarefaService service = new(tarefaRepository);

        await service.NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync(titulo, CancellationToken.None);

        await tarefaRepository.Received(1).ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            titulo,
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync_LancarException()
    {
        int id = 1;
        string titulo = "Título da Tarefa";
        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            id,
            titulo,
            Arg.Any<CancellationToken>()
        ).Returns(true);

        ValidarTituloTarefaService service = new(tarefaRepository);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async ()
            => await service.NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync(id, titulo, CancellationToken.None));

        Assert.Equal("Já existe uma tarefa com esse título em aberto.", exception.Message);

        await tarefaRepository.Received(1).ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            id,
            titulo,
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync_LancarException()
    {
        string titulo = "Título da Tarefa";
        ITarefaRepository tarefaRepository = Substitute.For<ITarefaRepository>();

        tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            titulo,
            Arg.Any<CancellationToken>()
        ).Returns(true);

        ValidarTituloTarefaService service = new(tarefaRepository);

        ValidacaoException exception = await Assert.ThrowsAsync<ValidacaoException>(async ()
            => await service.NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync(titulo, CancellationToken.None));

        Assert.Equal("Já existe uma tarefa com esse título em aberto.", exception.Message);

        await tarefaRepository.Received(1).ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            titulo,
            Arg.Any<CancellationToken>()
        );
    }
}
