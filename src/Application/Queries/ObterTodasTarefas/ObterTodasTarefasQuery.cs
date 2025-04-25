using Application.DTOs;
using Application.Model;
using MediatR;

namespace Application.Queries.ObterTodasTarefas;

public record ObterTodasTarefasQuery : IRequest<Result<IEnumerable<TarefaDto>>>;
