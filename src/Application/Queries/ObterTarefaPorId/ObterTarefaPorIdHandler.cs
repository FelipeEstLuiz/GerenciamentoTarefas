using Application.DTOs;
using Application.Model;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.Queries.ObterTarefaPorId;

public class ObterTarefaPorIdHandler(ITarefaRepository tarefaRepository)
    : IRequestHandler<ObterTarefaPorIdQuery, Result<TarefaDto>>
{
    public async Task<Result<TarefaDto>> Handle(ObterTarefaPorIdQuery request, CancellationToken cancellationToken)
    {
        Tarefa tarefa = await tarefaRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ValidacaoException("Tarefa não encontrada", HttpStatusCode.NotFound);

        return TarefaDto.Map(tarefa);
    }
}

