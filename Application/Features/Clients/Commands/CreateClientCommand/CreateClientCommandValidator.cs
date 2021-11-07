using FluentValidation;

namespace Application.Features.Clients.Commands.CreateClientCommand
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.Birthday)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .Matches(@"^\d{2}-\d{4}-\d{4}$").WithMessage("{PropertyName} debe tener el formato 00-0000-0000.")
                .MaximumLength(20).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .EmailAddress().WithMessage("{PropertyName} debe ser un email válido.")
                .MaximumLength(100).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.Address)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(120).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");
        }
    }
}
