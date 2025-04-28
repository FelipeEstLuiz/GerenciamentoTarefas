using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Commands.AtualizarTarefa;

public class AtualizarTarefaHandler(
    ITarefaRepository tarefaRepository,
    IObterTarefaService obterTarefaService,
    IValidarTituloTarefaService validarTituloTarefaService
)
    : IRequestHandler<AtualizarTarefaCommand, TarefaDto>
{
    public async Task<TarefaDto> Handle(AtualizarTarefaCommand request, CancellationToken cancellationToken)
    {
        await validarTituloTarefaService.NaoExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdOrThrowAsync(
            request.Id!.Value,
            request.Titulo!,
            cancellationToken
        );

        Tarefa tarefa = await obterTarefaService.ObterTarefaPorIdOrThrow(request.Id!.Value, cancellationToken);

        tarefa.Update(
            request.Titulo!,
            request.Descricao,
            request.StatusTarefa!.Value,
            request.DataConclusao
        );

        await tarefaRepository.UpdateAsync(tarefa, cancellationToken);

        return TarefaDto.Map(tarefa);
    }
}
