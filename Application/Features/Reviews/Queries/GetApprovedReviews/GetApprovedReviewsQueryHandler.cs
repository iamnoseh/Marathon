using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Queries.GetApprovedReviews;

public class GetApprovedReviewsQueryHandler : IRequestHandler<GetApprovedReviewsQuery, Response<List<ReviewDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetApprovedReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<List<ReviewDto>>> Handle(GetApprovedReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.IsApproved && !r.IsDeleted)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserFullName = r.User.FullName,
                UserProfilePicture = r.User.ProfilePicture ?? string.Empty,
                Text = r.Text,
                IsApproved = r.IsApproved,
                CreatedAt = r.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new Response<List<ReviewDto>>(reviews);
    }
}
