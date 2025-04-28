using Application.DTOs;
using MediatR;

namespace Application.Queries.ObterTarefaPorId;

public record ObterTarefaPorIdQuery(int Id) : IRequest<TarefaDto>;
