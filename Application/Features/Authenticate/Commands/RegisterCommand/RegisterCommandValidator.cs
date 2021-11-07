using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.LastName)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(80).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .EmailAddress().WithMessage("{PropertyName} debe ser un email válido.")
                .MaximumLength(100).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(15).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.");

            RuleFor(c => c.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} no puede ser vacio.")
                .MaximumLength(15).WithMessage("{PropertyName} no puede superar los {MaxLength} caracteres.")
                .Equal(p => p.Password).WithMessage("{PropertyName} debe ser igual a Password");
        }
    }
}
