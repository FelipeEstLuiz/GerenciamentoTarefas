using Domain.Entities;

namespace Domain.Services;

public interface IObterTarefaService
{
    Task<Tarefa> ObterTarefaPorIdOrThrow(int id, CancellationToken cancellationToken);
}
