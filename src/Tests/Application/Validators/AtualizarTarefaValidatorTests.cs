using Application.Commands.AtualizarTarefa;
using Application.Validators;
using Domain.Enums;

namespace Tests.Application.Validators;
public class AtualizarTarefaValidatorTests
{
    [Fact]
    public void AtualizarTarefaCommand_ValidarComando_Valido()
    {
        AtualizarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = "Descricao",
            Id = 1,
            DataConclusao = DateTime.Now,
            StatusTarefa = StatusTarefa.Concluida
        };

        AtualizarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.True(resultado.IsValid);
    }

    [Theory(DisplayName = "AtualizarTarefaCommand_ValidarComando_IdInvalido")]
    [InlineData(0)]
    [InlineData(null)]
    public void AtualizarTarefaCommand_ValidarComando_IdInvalido(int? id)
    {
        AtualizarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = "Descricao",
            Id = id,
            DataConclusao = DateTime.Now,
            StatusTarefa = StatusTarefa.Concluida
        };

        AtualizarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.Id));
    }

    [Fact]
    public void AtualizarTarefaCommand_ValidarComando_DataConclusao_QuandoStatusConcluida()
    {
        AtualizarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = "Descricao",
            Id = 1,
            StatusTarefa = StatusTarefa.Concluida
        };

        AtualizarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.DataConclusao));
        Assert.Contains(resultado.Errors, x => x.ErrorMessage.Contains("Obrigatória quando o status for Concluída", StringComparison.CurrentCultureIgnoreCase));
    }

    [Fact]
    public void AtualizarTarefaCommand_ValidarComando_StatusTarefa_QuandoEnviarDataConclusao()
    {
        AtualizarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = "Descricao",
            Id = 1,
            StatusTarefa = StatusTarefa.EmProgresso,
            DataConclusao = DateTime.Now
        };

        AtualizarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.StatusTarefa));
        Assert.Contains(resultado.Errors, x => x.ErrorMessage.Contains("Deve ser Concluída ao enviar a data de conclusao", StringComparison.CurrentCultureIgnoreCase));
    }

    [Theory(DisplayName = "AtualizarTarefaCommand_ValidarComando_StatusTarefaInvalido")]
    [InlineData(10)]
    [InlineData(null)]
    public void AtualizarTarefaCommand_ValidarComando_StatusTarefaInvalido(int? statusTarefa)
    {
        AtualizarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = "Descricao",
            Id = 1,
            DataConclusao = DateTime.Now,
            StatusTarefa = (StatusTarefa?)statusTarefa
        };

        AtualizarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.StatusTarefa));
    }
}
