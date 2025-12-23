using FluentValidation;
using Application.Constants;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommandValidator : AbstractValidator<SubmitMarathonAttemptCommand>
{
    public SubmitMarathonAttemptCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(Messages.Marathon.UserIdRequired);

        RuleFor(x => x.Score)
            .GreaterThanOrEqualTo(0).WithMessage(Messages.Marathon.ScoreInvalid);
    }
}
