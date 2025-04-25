using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

public class Tarefa
{
    public Tarefa() { }

    public Tarefa(string titulo, string? descricao) => ValidateDomain(titulo, descricao);

    public Tarefa(
        int id,
        string titulo,
        string? descricao,
        StatusTarefa statusTarefa,
        DateTime dataCriacao,
        DateTime? dataConclusao
    )
    {
        ValidacaoException.When(id < 0, "Id inválido");
        ValidateDomain(titulo, descricao);

        ValidacaoException.When(
            dataConclusao.HasValue && dataConclusao.Value < dataCriacao,
            "Data de Conclusão não pode ser anterior à Data de Criação"
        );

        Id = id;
        CodigoStatusTarefa = statusTarefa;
        DataConclusao = dataConclusao;
        DataCriacao = dataCriacao;
    }

    public int Id { get; private set; }
    public string Titulo { get; private set; } = null!;
    public string? Descricao { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public StatusTarefa CodigoStatusTarefa { get; private set; }
    public DateTime? DataConclusao { get; private set; }

    public void Update(
        string titulo,
        string? descricao,
        StatusTarefa statusTarefa,
        DateTime? dataConclusao
    )
    {
        ValidateDomain(titulo, descricao);
        SetStatus(statusTarefa);
        SetDataConclusao(dataConclusao);

        ValidacaoException.When(
            !dataConclusao.HasValue && statusTarefa == StatusTarefa.Concluida,
            "Data de Conclusão é obrigatório ao concluir a tarefa"
        );

        ValidacaoException.When(
            dataConclusao.HasValue && statusTarefa != StatusTarefa.Concluida,
            "Data de Conclusão só deve ser enviada ao concluir uma tarefa"
        );
    }

    public void SetStatus(StatusTarefa status)
    {
        ValidacaoException.When(
            CodigoStatusTarefa == StatusTarefa.Concluida,
            "A tarefa não pode ser editada pois está Concluida"
        );

        CodigoStatusTarefa = status;
    }

    public void SetId(int id) => Id = id;

    public void SetDataCriacao(DateTime dataCriacao) => DataCriacao = dataCriacao;

    public void SetDataConclusao(DateTime? dataConclusao)
    {
        ValidacaoException.When(
            dataConclusao.HasValue && dataConclusao.Value < DataCriacao,
            "Data de Conclusão não pode ser anterior à Data de Criação"
        );
        DataConclusao = dataConclusao;
    }

    private void ValidateDomain(string titulo, string? descricao)
    {
        ValidacaoException.When(string.IsNullOrEmpty(titulo), "Título é obrigatorio");
        ValidacaoException.When(titulo.Length > 100, "Título deve conter no máximo 100 caracter");
        ValidacaoException.When(descricao is not null && descricao.Length > 500, "Título deve conter no máximo 500 caracter");

        Titulo = titulo;
        Descricao = descricao;
    }
}
