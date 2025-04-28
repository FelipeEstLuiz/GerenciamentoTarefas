using Application.DTOs;
using MediatR;

namespace Application.Commands;

public abstract record TarefaCommand : IRequest<TarefaDto>
{
    public string? Titulo { get; set; }
    public string? Descricao { get; set; }
}
