using Application.Commands.AtualizarTarefa;
using Domain.Enums;
using FluentValidation;

namespace Application.Validators;

public class AtualizarTarefaValidator : AbstractValidator<AtualizarTarefaCommand>
{
    public AtualizarTarefaValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Obrigatório.")
            .GreaterThan(0).WithMessage("Inválido");

        RuleFor(x => x.DataConclusao)
            .NotEmpty()
            .WithMessage("Obrigatória quando o status for Concluída")
            .When(x => x.StatusTarefa ==StatusTarefa.Concluida);

        RuleFor(x => x.StatusTarefa)
            .Equal(StatusTarefa.Concluida)
            .WithMessage("Deve ser Concluída ao enviar a data de conclusao")
            .When(x => x.DataConclusao.HasValue);

        RuleFor(x => x.StatusTarefa)
            .NotEmpty().WithMessage("Obrigatória")
            .IsInEnum().WithMessage("Inválido");

        RuleFor(x => x).SetValidator(new TarefaValidator());
    }
}
