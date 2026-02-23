using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Reviews.Queries.GetApprovedReviews;

public class GetApprovedReviewsQuery : IRequest<Response<List<ReviewDto>>>
{
}
