using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommandHandler(IApplicationDbContext context)
    : IRequestHandler<SubmitMarathonAttemptCommand, Response<BestResultDto>>
{
    public async Task<Response<BestResultDto>> Handle(SubmitMarathonAttemptCommand request, CancellationToken cancellationToken)
    {
        var attemptTime = DateTime.UtcNow;

        var attempt = new MarathonAttempt
        {
            UserId = request.UserId,
            Score = request.Score,
            AchievedAt = attemptTime,
            CreatedAt = attemptTime
        };

        await context.MarathonAttempts.AddAsync(attempt, cancellationToken);

        var currentBestResult = await context.BestResults
            .FirstOrDefaultAsync(br => br.UserId == request.UserId && !br.IsDeleted, cancellationToken);

        BestResult bestResult;

        if (currentBestResult == null)
        {
            bestResult = new BestResult
            {
                UserId = request.UserId,
                Score = request.Score,
                AchievedAt = attemptTime,
                CreatedAt = attemptTime
            };

            await context.BestResults.AddAsync(bestResult, cancellationToken);
        }
        else
        {
            bool shouldUpdate = false;

            if (request.Score > currentBestResult.Score)
            {
                shouldUpdate = true;
            }
            else if (request.Score == currentBestResult.Score && attemptTime < currentBestResult.AchievedAt)
            {
                shouldUpdate = true;
            }

            if (shouldUpdate)
            {
                currentBestResult.Score = request.Score;
                currentBestResult.AchievedAt = attemptTime;
                currentBestResult.UpdatedAt = attemptTime;
            }

            bestResult = currentBestResult;
        }

        await context.SaveChangesAsync(cancellationToken);

        var resultDto = new BestResultDto
        {
            Score = bestResult.Score,
            AchievedAt = bestResult.AchievedAt
        };

        return new Response<BestResultDto>(resultDto);
    }
}
