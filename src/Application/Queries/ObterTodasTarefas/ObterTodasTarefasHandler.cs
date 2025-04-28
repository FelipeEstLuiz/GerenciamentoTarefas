using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Queries.ObterTodasTarefas;

public class ObterTodasTarefasHandler(ITarefaRepository tarefaRepository)
    : IRequestHandler<ObterTodasTarefasQuery, ServerSideDto<IEnumerable<TarefaListagemDto>>>
{
    public async Task<ServerSideDto<IEnumerable<TarefaListagemDto>>> Handle(
        ObterTodasTarefasQuery request,
        CancellationToken cancellationToken
    )
    {
        ServerSide<IEnumerable<Tarefa>> tarefas = await tarefaRepository.GetAllAsync(
            request.Page,
            request.Limit,
            cancellationToken
        );

        ServerSideDto<IEnumerable<TarefaListagemDto>> serverSideDto = new()
        {
            Limit = request.Limit,
            Page = request.Page,
            Data = tarefas.Data?.Select(TarefaListagemDto.Map),
            TotalData = tarefas.TotalData,
            TotalPages = (int)Math.Ceiling((double)tarefas.TotalData / request.Limit)
        };

        return serverSideDto;
    }
}
