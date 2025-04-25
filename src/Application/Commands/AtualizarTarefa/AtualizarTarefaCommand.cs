using Domain.Enums;

namespace Application.Commands.AtualizarTarefa;

public record AtualizarTarefaCommand : TarefaCommand
{
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int? Id { get; set; }
    public StatusTarefa? StatusTarefa { get; set; }
    public DateTime? DataConclusao { get; set; }
}
