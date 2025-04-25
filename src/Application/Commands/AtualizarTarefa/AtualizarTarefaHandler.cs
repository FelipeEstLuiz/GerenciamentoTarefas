using Application.DTOs;
using Application.Model;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using MediatR;
using System.Net;

namespace Application.Commands.AtualizarTarefa;

public class AtualizarTarefaHandler(
    ITarefaRepository tarefaRepository,
    IValidarTituloTarefaService validarTituloTarefaService
)
    : IRequestHandler<AtualizarTarefaCommand, Result<TarefaDto>>
{
    public async Task<Result<TarefaDto>> Handle(AtualizarTarefaCommand request, CancellationToken cancellationToken)
    {
        await validarTituloTarefaService.ExisteTarefaComMesmoTituloNaoConcluidaDeOutroIdAsync(
            request.Id!.Value,
            request.Titulo!,
            cancellationToken
        );

        Tarefa tarefa = await tarefaRepository.GetByIdAsync(request.Id!.Value, cancellationToken)
            ?? throw new ValidacaoException("Tarefa não encontrada", HttpStatusCode.NotFound);

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
