using Application.Commands.CriarTarefa;
using Application.Validators;

namespace Tests.Application.Validators;

public class CriarTarefaValidatorTests
{
    [Theory(DisplayName = "CriarTarefaValidator_ValidarComando_Valido")]
    [InlineData("descricao")]
    [InlineData(null)]
    public void CriarTarefaValidator_ValidarComando_Valido(string? descricao)
    {
        CriarTarefaCommand comando = new()
        {
            Titulo = "Teste",
            Descricao = descricao
        };

        CriarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.True(resultado.IsValid);
    }

    [Fact]
    public void CriarTarefaValidator_ValidarComando_TituloInvalido_Maior100Caracteres()
    {
        CriarTarefaCommand comando = new()
        {
            Titulo = new string('x', 101)
        };

        CriarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.Titulo));
        Assert.Contains(resultado.Errors, x => x.ErrorMessage.Contains("Deve ter no máximo 100 caracteres", StringComparison.CurrentCultureIgnoreCase));
    }

    [Fact]
    public void CriarTarefaValidator_ValidarComando_TituloInvalido_Vazio()
    {
        CriarTarefaCommand comando = new()
        {
            Titulo = string.Empty
        };

        CriarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.Titulo));
        Assert.Contains(resultado.Errors, x => x.ErrorMessage.Contains("Obrigatório", StringComparison.CurrentCultureIgnoreCase));
    }

    [Fact]
    public void CriarTarefaValidator_ValidarComando_DescricaoInvalido_Maior500Caracteres()
    {
        CriarTarefaCommand comando = new()
        {
            Titulo = "Titulo",
            Descricao = new string('x', 501)
        };

        CriarTarefaValidator validator = new();

        FluentValidation.Results.ValidationResult resultado = validator.Validate(comando);

        Assert.False(resultado.IsValid);
        Assert.Contains(resultado.Errors, x => x.PropertyName == nameof(comando.Descricao));
        Assert.Contains(resultado.Errors, x => x.ErrorMessage.Contains("Deve ter no máximo 500 caracteres.", StringComparison.CurrentCultureIgnoreCase));
    }
}
