using FluentValidation;
using Application.Constants;

namespace Application.Features.Auth.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Messages.Auth.EmailRequired)
            .EmailAddress().WithMessage(Messages.Auth.EmailInvalid);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Messages.Auth.PasswordRequired);
    }
}
