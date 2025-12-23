using FluentValidation;
using Application.Constants;

namespace Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage(Messages.Auth.FullNameRequired)
            .MinimumLength(3).WithMessage(Messages.Auth.FullNameTooShort);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Messages.Auth.EmailRequired)
            .EmailAddress().WithMessage(Messages.Auth.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.Auth.PasswordRequired)
            .MinimumLength(6).WithMessage(Messages.Auth.PasswordTooShort);
    }
}
