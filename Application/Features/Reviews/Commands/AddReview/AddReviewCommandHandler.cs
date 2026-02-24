using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;

namespace Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Response<int>>
{
    private readonly IApplicationDbContext _context;

    public AddReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<int>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Review
        {
            UserId = request.UserId,
            Text = request.Text,
            Rating = request.Rating,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Reviews.AddAsync(review, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new Response<int>(review.Id, "Отзыв успешно отправлен и ожидает модерации.");
    }
}
