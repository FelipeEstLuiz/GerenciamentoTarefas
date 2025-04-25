using Application.Model;
using MediatR;

namespace Application.Commands.DeletarTarefa;

public record DeletarTarefaCommand(int Id) : IRequest<Result<string>>;
