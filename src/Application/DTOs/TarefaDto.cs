using Domain.Entities;

namespace Application.DTOs;

public class TarefaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; } 
    public int CodigoStatusTarefa { get; set; } 
    public DateTime? DataConclusao { get; set; }

    public static TarefaDto Map(Tarefa tarefa) => new()
    {
        Id = tarefa.Id,
        Titulo = tarefa.Titulo,
        Descricao = tarefa.Descricao,
        CodigoStatusTarefa = (int)tarefa.CodigoStatusTarefa,
        DataCriacao = tarefa.DataCriacao,
        DataConclusao = tarefa.DataConclusao
    };
}
