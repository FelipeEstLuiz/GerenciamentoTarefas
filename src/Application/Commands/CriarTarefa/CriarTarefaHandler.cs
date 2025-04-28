using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Commands.CriarTarefa;

public class CriarTarefaHandler(
    ITarefaRepository tarefaRepository,
    IValidarTituloTarefaService validarTituloTarefaService
) : IRequestHandler<CriarTarefaCommand, TarefaDto>
{
    public async Task<TarefaDto> Handle(CriarTarefaCommand request, CancellationToken cancellationToken)
    {
        await validarTituloTarefaService.NaoExisteTarefaComMesmoTituloNaoConcluidaOrThrowAsync(
            request.Titulo!,
            cancellationToken
        );

        Tarefa tarefa = new(request.Titulo!, request.Descricao);
        await tarefaRepository.AddAsync(tarefa, cancellationToken);

        return TarefaDto.Map(tarefa);
    }
}

