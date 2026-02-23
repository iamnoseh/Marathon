using Application.Responses;
using MediatR;

namespace Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommand : IRequest<Response<int>>
{
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
