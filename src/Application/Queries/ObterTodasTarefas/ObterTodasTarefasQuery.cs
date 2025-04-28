using Application.DTOs;
using MediatR;

namespace Application.Queries.ObterTodasTarefas;

public record ObterTodasTarefasQuery(int Page, int Limit) : IRequest<ServerSideDto<IEnumerable<TarefaListagemDto>>>;
