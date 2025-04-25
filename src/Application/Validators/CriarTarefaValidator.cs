using Application.Commands.CriarTarefa;
using FluentValidation;

namespace Application.Validators;

public class CriarTarefaValidator : AbstractValidator<CriarTarefaCommand>
{
    public CriarTarefaValidator() => RuleFor(x => x).SetValidator(new TarefaValidator());
}
