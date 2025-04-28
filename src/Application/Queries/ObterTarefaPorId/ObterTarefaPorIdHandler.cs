using Application.DTOs;
using Domain.Entities;
using Domain.Services;
using MediatR;

namespace Application.Queries.ObterTarefaPorId;

public class ObterTarefaPorIdHandler(IObterTarefaService obterTarefaService)
    : IRequestHandler<ObterTarefaPorIdQuery, TarefaDto>
{
    public async Task<TarefaDto> Handle(ObterTarefaPorIdQuery request, CancellationToken cancellationToken)
    {
        Tarefa tarefa = await obterTarefaService.ObterTarefaPorIdOrThrow(request.Id, cancellationToken);
        return TarefaDto.Map(tarefa);
    }
}

