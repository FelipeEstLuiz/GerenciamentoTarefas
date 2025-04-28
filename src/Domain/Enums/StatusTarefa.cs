using Domain.Attributes;

namespace Domain.Enums;
public enum StatusTarefa
{
    [EnumName("Pendente")]
    Pendente = 0,
    
    [EnumName("Em Progresso")]
    EmProgresso = 1,

    [EnumName("Concluída")]
    Concluida = 2
}
