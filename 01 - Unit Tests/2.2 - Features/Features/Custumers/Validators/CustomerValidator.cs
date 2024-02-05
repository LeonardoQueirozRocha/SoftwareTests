using Features.Custumers.Models;
using FluentValidation;

namespace Features.Custumers.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    private const int MinimumLength = 2;
    private const int MaximumLength = 150;
    private const int MinimumAge = -18;

    public CustomerValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Por favor, certifique-se de ter inserido o nome")
            .Length(MinimumLength, MaximumLength)
                .WithMessage($"O nome deve ter entre {MinimumLength} e {MaximumLength} caracteres");

        RuleFor(c => c.Name)
            .NotEmpty()
                .WithMessage("Por favor, certifique-se de ter inserido o sobrenome")
            .Length(MinimumLength, MaximumLength)
                .WithMessage($"O sobrenome deve ter entre {MinimumLength} e {MaximumLength} caracteres");

        RuleFor(c => c.BirthDate)
            .NotEmpty()
            .Must(HaveMinimumAge)
                .WithMessage("O cliente deve ter 18 anos ou mais");

        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }

    private static bool HaveMinimumAge(DateTime birthDate) =>
        birthDate.Date.CompareTo(DateTime.Now.Date.AddYears(MinimumAge)) <= 0;
}