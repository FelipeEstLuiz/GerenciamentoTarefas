using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services;

public class ValidarTituloTarefaService(ITarefaRepository tarefaRepository) : IValidarTituloTarefaService
{
    public async Task ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
        int id,
        string titulo,
        CancellationToken cancellationToken
    ) => ThrowIfExists(await tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
        id,
        titulo,
        cancellationToken
    ));

    public async Task ExisteTarefaComMesmoTituloNaoConcluidaAsync(
        string titulo,
        CancellationToken cancellationToken
    ) => ThrowIfExists(await tarefaRepository.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
        titulo,
        cancellationToken
    ));

    private static void ThrowIfExists(bool result) => ValidacaoException.When(
        result,
        "Já existe uma tarefa com esse título em aberto."
    );
}
