using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using System.Net;

namespace Application.Services;
public class ObterTarefaService(ITarefaRepository tarefaRepository) : IObterTarefaService
{
    public async Task<Tarefa> ObterTarefaPorIdOrThrow(int id, CancellationToken cancellationToken)
        => await tarefaRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ValidacaoException("Tarefa não encontrada", HttpStatusCode.NotFound);
}
