using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommand : IRequest<Response<BestResultDto>>
{
    public string UserId { get; set; } = string.Empty;
    public int Score { get; set; }
}
