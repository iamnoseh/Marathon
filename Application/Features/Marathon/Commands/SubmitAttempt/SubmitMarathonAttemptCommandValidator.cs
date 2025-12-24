using FluentValidation;
using Application.Constants;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommandValidator : AbstractValidator<SubmitMarathonAttemptCommand>
{
    public SubmitMarathonAttemptCommandValidator()
    {

        RuleFor(x => x.FrontendScore)
            .GreaterThanOrEqualTo(0).WithMessage(Messages.Marathon.ScoreInvalid);

        RuleFor(x => x.BackendScore)
            .GreaterThanOrEqualTo(0).WithMessage(Messages.Marathon.ScoreInvalid);
    }
}
