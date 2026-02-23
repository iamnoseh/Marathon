using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Reviews.Queries.GetPendingReviews;

public class GetPendingReviewsQuery : IRequest<Response<List<ReviewDto>>>
{
}
