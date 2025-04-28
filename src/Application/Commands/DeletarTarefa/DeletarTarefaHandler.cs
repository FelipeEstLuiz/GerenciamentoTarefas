using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;
using MediatR;

namespace Application.Commands.DeletarTarefa;

public class DeletarTarefaHandler(
    ITarefaRepository tarefaRepository,
    IObterTarefaService obterTarefaService
) : IRequestHandler<DeletarTarefaCommand, string>
{
    public async Task<string> Handle(DeletarTarefaCommand request, CancellationToken cancellationToken)
    {
        Tarefa tarefa = await obterTarefaService.ObterTarefaPorIdOrThrow(request.Id, cancellationToken);

        ValidacaoException.When(
            tarefa.CodigoStatusTarefa != StatusTarefa.Pendente,
            "A tarefa não pode ser removida pois não está pendente"
        );

        await tarefaRepository.DeleteAsync(request.Id, cancellationToken);

        return "Tarefa removida com sucesso";
    }
}
