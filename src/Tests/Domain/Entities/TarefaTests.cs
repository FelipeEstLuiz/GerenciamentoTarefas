using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Tests.Domain.Entities;

public class TarefaTests
{
    [Fact]
    public void Tarefa_ConstrutorSimples_ValidaDados()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";

        Tarefa tarefa = new(titulo, descricao);

        Assert.Equal(titulo, tarefa.Titulo);
        Assert.Equal(descricao, tarefa.Descricao);
        Assert.Null(tarefa.DataConclusao);
    }

    [Fact]
    public void Tarefa_Construtor_ValidaDados()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        Assert.Equal(titulo, tarefa.Titulo);
        Assert.Equal(descricao, tarefa.Descricao);
        Assert.Equal(id, tarefa.Id);
        Assert.Equal(statusTarefa, tarefa.CodigoStatusTarefa);
        Assert.Equal(dataCriacao, tarefa.DataCriacao);
        Assert.Equal(dataConclusao, tarefa.DataConclusao);
    }

    [Fact]
    public void Tarefa_Update_ValidaDados()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.EmProgresso;
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);
        DateTime? dataConclusao = null;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        statusTarefa = StatusTarefa.Concluida;
        dataConclusao = DateTime.UtcNow;

        tarefa.Update(
            titulo,
            descricao,
            statusTarefa,
            dataConclusao
        );

        Assert.Equal(titulo, tarefa.Titulo);
        Assert.Equal(descricao, tarefa.Descricao);
        Assert.Equal(statusTarefa, tarefa.CodigoStatusTarefa);
        Assert.Equal(dataConclusao, tarefa.DataConclusao);
    }

    [Fact]
    public void Tarefa_SetStatus_ValidaDados()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.EmProgresso;
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        statusTarefa = StatusTarefa.Concluida;

        tarefa.SetStatus(statusTarefa);

        Assert.Equal(statusTarefa, tarefa.CodigoStatusTarefa);
    }

    [Fact]
    public void Tarefa_SetId_ValidaDados()
    {
        int id = 1;
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";

        Tarefa tarefa = new(titulo, descricao);

        tarefa.SetId(id);

        Assert.Equal(id, tarefa.Id);
    }

    [Fact]
    public void Tarefa_SetDataCriacao_ValidaDados()
    {
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);

        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";

        Tarefa tarefa = new(titulo, descricao);

        tarefa.SetDataCriacao(dataCriacao);

        Assert.Equal(dataCriacao, tarefa.DataCriacao);
    }

    [Fact]
    public void Tarefa_SetDataConclusao_ValidaDados()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(titulo, descricao);

        tarefa.SetDataConclusao(dataConclusao);

        Assert.Equal(dataConclusao, tarefa.DataConclusao);
    }

    [Fact]
    public void Tarefa_ConstrutorSimples_ValidaDados_TituloInvalido_Vazio()
    {
        string? descricao = "Descrição da Tarefa";

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => new Tarefa(string.Empty, descricao));

        Assert.Equal("Título é obrigatorio", exception.Message);
    }

    [Fact]
    public void Tarefa_ConstrutorSimples_ValidaDados_TituloInvalido_Maior100Caracter()
    {
        string titulo = new('x', 101);
        string? descricao = "Descrição da Tarefa";

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => new Tarefa(titulo, descricao));

        Assert.Equal("Título deve conter no máximo 100 caracteres", exception.Message);
    }

    [Fact]
    public void Tarefa_ConstrutorSimples_ValidaDados_DescricaoInvalido_Maior500Caracter()
    {
        string titulo = "Título da Tarefa";
        string? descricao = new('x', 501);

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => new Tarefa(titulo, descricao));

        Assert.Equal("Descrição deve conter no máximo 500 caracteres", exception.Message);
    }

    [Fact]
    public void Tarefa_Construtor_ValidaDados_IdInvalido()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 0;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow.AddHours(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => new Tarefa(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        ));

        Assert.Equal("Id inválido", exception.Message);
    }

    [Fact]
    public void Tarefa_Construtor_ValidaDados_DataConclusaoMaiorDataCriacao()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.EmProgresso;
        DateTime dataCriacao = DateTime.UtcNow;
        DateTime? dataConclusao = DateTime.UtcNow.AddMinutes(-1);

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => new Tarefa(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        ));

        Assert.Equal("Data de Conclusão não pode ser anterior à Data de Criação", exception.Message);
    }

    [Fact]
    public void Tarefa_Update_ValidaDados_Status()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow.AddMinutes(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => tarefa.Update(
            titulo,
            descricao,
            StatusTarefa.EmProgresso,
            dataConclusao
        ));

        Assert.Equal("A tarefa não pode ser editada pois está Concluida", exception.Message);
    }

    [Fact]
    public void Tarefa_Update_ValidaDados_DataConclusaoNaoEnviadaEStatusConcluida()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.EmProgresso;
        DateTime dataCriacao = DateTime.UtcNow.AddMinutes(-1);
        DateTime? dataConclusao = null;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => tarefa.Update(
            titulo,
            descricao,
            StatusTarefa.Concluida,
            dataConclusao
        ));

        Assert.Equal("Data de Conclusão é obrigatório ao concluir a tarefa", exception.Message);
    }

    [Fact]
    public void Tarefa_Update_ValidaDados_DataConclusaoEnviadaEStatusNaoConcluida()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.EmProgresso;
        DateTime dataCriacao = DateTime.UtcNow.AddMinutes(-1);
        DateTime? dataConclusao = null;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => tarefa.Update(
            titulo,
            descricao,
            StatusTarefa.EmProgresso,
            DateTime.UtcNow.AddMinutes(1)
        ));

        Assert.Equal("Data de Conclusão só deve ser enviada ao concluir uma tarefa", exception.Message);
    }

    [Fact]
    public void Tarefa_SetStatus_ValidaDados_Invalido()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow.AddMinutes(-1);
        DateTime? dataConclusao = DateTime.UtcNow;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => tarefa.SetStatus(StatusTarefa.EmProgresso));

        Assert.Equal("A tarefa não pode ser editada pois está Concluida", exception.Message);
    }

    [Fact]
    public void Tarefa_SetDataCriacao_ValidaDados_Invalido()
    {
        string titulo = "Título da Tarefa";
        string? descricao = "Descrição da Tarefa";
        int id = 1;
        StatusTarefa statusTarefa = StatusTarefa.Concluida;
        DateTime dataCriacao = DateTime.UtcNow;
        DateTime? dataConclusao = null;

        Tarefa tarefa = new(
            id,
            titulo,
            descricao,
            statusTarefa,
            dataCriacao,
            dataConclusao
        );

        ValidacaoException exception = Assert.Throws<ValidacaoException>(() => tarefa.SetDataConclusao(DateTime.UtcNow.AddMinutes(-1)));

        Assert.Equal("Data de Conclusão não pode ser anterior à Data de Criação", exception.Message);
    }
}
