namespace Domain.Services;

public interface IValidarTituloTarefaService
{
    Task ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
        int id,
        string titulo,
        CancellationToken cancellationToken
    );
    Task ExisteTarefaComMesmoTituloNaoConcluidaAsync(
        string titulo,
        CancellationToken cancellationToken
    );
}
