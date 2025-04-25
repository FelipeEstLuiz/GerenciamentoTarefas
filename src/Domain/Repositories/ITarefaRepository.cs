using Domain.Entities;

namespace Domain.Repositories;

public interface ITarefaRepository
{
    Task<Tarefa?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExisteTarefaComMesmoTituloNaoConcluidaAsync(string titulo, CancellationToken cancellationToken);
    Task<bool> ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
        int id,
        string titulo,
        CancellationToken cancellationToken
    );
    Task<Tarefa> UpdateAsync(Tarefa tarefa, CancellationToken cancellationToken);
    Task<Tarefa> AddAsync(Tarefa tarefa, CancellationToken cancellationToken);
    Task<IEnumerable<Tarefa>> GetAllAsync(CancellationToken cancellationToken);
}
