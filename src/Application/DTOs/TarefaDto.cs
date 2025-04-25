using Domain.Entities;

namespace Application.DTOs;

public class TarefaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public string DataCriacao { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? DataConclusao { get; set; }

    public static TarefaDto Map(Tarefa tarefa) => new()
    {
        Id = tarefa.Id,
        Titulo = tarefa.Titulo,
        Descricao = tarefa.Descricao,
        Status = tarefa.CodigoStatusTarefa.ToString(),
        DataCriacao = tarefa.DataCriacao.ToString("dd/MM/yyyy HH:mm:ss"),
        DataConclusao = tarefa.DataConclusao?.ToString("dd/MM/yyyy HH:mm:ss")
    };
}
