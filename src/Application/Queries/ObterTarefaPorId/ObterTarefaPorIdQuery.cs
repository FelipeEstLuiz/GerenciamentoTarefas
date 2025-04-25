using Application.DTOs;
using Application.Model;
using MediatR;

namespace Application.Queries.ObterTarefaPorId;

public record ObterTarefaPorIdQuery(int Id) : IRequest<Result<TarefaDto>>;
