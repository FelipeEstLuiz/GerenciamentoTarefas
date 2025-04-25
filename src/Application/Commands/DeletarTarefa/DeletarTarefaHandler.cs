using Application.Model;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.Commands.DeletarTarefa;

public class DeletarTarefaHandler(ITarefaRepository tarefaRepository) : IRequestHandler<DeletarTarefaCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeletarTarefaCommand request, CancellationToken cancellationToken)
    {
        Tarefa tarefa = await tarefaRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new ValidacaoException("Tarefa não encontrada", HttpStatusCode.NotFound);

        ValidacaoException.When(
            tarefa.CodigoStatusTarefa != StatusTarefa.Pendente,
            "A tarefa não pode ser removida pois não está pendente"
        );

        await tarefaRepository.DeleteAsync(request.Id, cancellationToken);

        return Result<string>.Success("Tarefa removida com sucesso");
    }
}
