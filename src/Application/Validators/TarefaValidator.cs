using Application.Commands;
using FluentValidation;

namespace Application.Validators;

public class TarefaValidator : AbstractValidator<TarefaCommand>
{
    public TarefaValidator()
    {
        RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("Obrigatório.")
            .MaximumLength(100)
            .WithMessage("Deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500).WithMessage("Deve ter no máximo 500 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Descricao));
    }
}
