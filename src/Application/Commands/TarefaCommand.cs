using Application.DTOs;
using Application.Model;
using MediatR;

namespace Application.Commands;

public abstract record TarefaCommand : IRequest<Result<TarefaDto>>
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
}
