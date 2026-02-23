using System.Net;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Reviews.Commands.ApproveReview;

public class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommand, Response<Unit>>
{
    private readonly IApplicationDbContext _context;

    public ApproveReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<Unit>> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId && !r.IsDeleted, cancellationToken);

        if (review == null)
        {
            return new Response<Unit>(HttpStatusCode.NotFound, "Отзыв не найден.");
        }

        review.IsApproved = true;
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<Unit>(Unit.Value, "Отзыв успешно одобрен.");
    }
}
