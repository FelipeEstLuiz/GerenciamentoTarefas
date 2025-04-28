namespace Domain.Services;

public interface IValidarTituloTarefaService
{
    Task NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync(
        int id,
        string titulo,
        CancellationToken cancellationToken
    );
    Task NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync(
        string titulo,
        CancellationToken cancellationToken
    );
}
