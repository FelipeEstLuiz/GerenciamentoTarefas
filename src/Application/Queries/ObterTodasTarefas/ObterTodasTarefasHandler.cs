using Application.DTOs;
using Application.Model;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.ObterTodasTarefas;

public class ObterTodasTarefasHandler(ITarefaRepository tarefaRepository) 
    : IRequestHandler<ObterTodasTarefasQuery, Result<IEnumerable<TarefaDto>>>
{
    public async Task<Result<IEnumerable<TarefaDto>>> Handle(
        ObterTodasTarefasQuery request,
        CancellationToken cancellationToken
    )
    {
        IEnumerable<Tarefa> tarefa = await tarefaRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<TarefaDto>>.Success(tarefa.Select(TarefaDto.Map));
    }
}
