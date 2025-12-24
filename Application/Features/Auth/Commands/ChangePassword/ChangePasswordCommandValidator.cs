using FluentValidation;
using Application.Constants;

namespace Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage(Messages.Auth.CurrentPasswordRequired);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(Messages.Auth.NewPasswordRequired)
            .MinimumLength(6).WithMessage(Messages.Auth.PasswordTooShort);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.NewPassword).WithMessage(Messages.Auth.PasswordsDoNotMatch);
    }
}
