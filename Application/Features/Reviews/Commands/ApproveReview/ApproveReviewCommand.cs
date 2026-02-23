using Application.Responses;
using MediatR;

namespace Application.Features.Reviews.Commands.ApproveReview;

public class ApproveReviewCommand : IRequest<Response<Unit>>
{
    public int ReviewId { get; set; }
}
