using Application.DTOs;
using Application.Model;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Commands.CriarTarefa;

public class CriarTarefaHandler(
    ITarefaRepository tarefaRepository,
    IValidarTituloTarefaService validarTituloTarefaService
) : IRequestHandler<CriarTarefaCommand, Result<TarefaDto>>
{
    public async Task<Result<TarefaDto>> Handle(CriarTarefaCommand request, CancellationToken cancellationToken)
    {
        await validarTituloTarefaService.ExisteTarefaComMesmoTituloNaoConcluidaAsync(
            request.Titulo!,
            cancellationToken
        );

        Tarefa tarefa = new(request.Titulo!, request.Descricao);
        await tarefaRepository.AddAsync(tarefa, cancellationToken);

        return TarefaDto.Map(tarefa);
    }
}

