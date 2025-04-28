using Domain.Converter;
using Domain.Entities;
using Domain.Enums;

namespace Application.DTOs;

public class TarefaListagemDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Descricao { get; set; }

    [Newtonsoft.Json.JsonConverter(typeof(ConvertDateTime))]
    public DateTime DataCriacao { get; set; }

    [Newtonsoft.Json.JsonConverter(typeof(EnumToStringConverter<StatusTarefa>))]
    public StatusTarefa Status { get; set; }


    [Newtonsoft.Json.JsonConverter(typeof(ConvertDateTime))]
    public DateTime? DataConclusao { get; set; }

    public bool PermiteRemover => Status != StatusTarefa.Concluida;

    public static TarefaListagemDto Map(Tarefa tarefa) => new()
    {
        Id = tarefa.Id,
        Titulo = tarefa.Titulo,
        Descricao = tarefa.Descricao,
        Status = tarefa.CodigoStatusTarefa,
        DataCriacao = tarefa.DataCriacao,
        DataConclusao = tarefa.DataConclusao
    };
}
