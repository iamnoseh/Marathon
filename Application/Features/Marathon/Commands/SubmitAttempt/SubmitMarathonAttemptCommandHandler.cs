using System.Net;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommandHandler : IRequestHandler<SubmitMarathonAttemptCommand, Response<BestResultDto>>
{
    private readonly IApplicationDbContext _context;

    public SubmitMarathonAttemptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<BestResultDto>> Handle(SubmitMarathonAttemptCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            
        if (user == null || user.IsBlocked)
        {
            return new Response<BestResultDto>(HttpStatusCode.Forbidden, "Ваш аккаунт заблокирован или не найден.");
        }

        var attemptTime = DateTime.UtcNow;

        var attempt = new MarathonAttempt
        {
            UserId = request.UserId,
            FrontendScore = request.FrontendScore,
            BackendScore = request.BackendScore,
            MobdevScore = request.MobdevScore,
            AchievedAt = attemptTime,
            CreatedAt = attemptTime
        };

        await _context.MarathonAttempts.AddAsync(attempt, cancellationToken);

        var currentBestResult = await _context.BestResults
            .FirstOrDefaultAsync(br => br.UserId == request.UserId && !br.IsDeleted, cancellationToken);

        BestResult bestResult;

        if (currentBestResult == null)
        {
            bestResult = new BestResult
            {
                UserId = request.UserId,
                BestFrontendScore = request.FrontendScore,
                BestBackendScore = request.BackendScore,
                BestMobdevScore = request.MobdevScore,
                FrontendAchievedAt = attemptTime,
                BackendAchievedAt = attemptTime,
                MobdevAchievedAt = attemptTime,
                CreatedAt = attemptTime
            };

            await _context.BestResults.AddAsync(bestResult, cancellationToken);
        }
        else
        {
            bool updated = false;

            if (request.FrontendScore > currentBestResult.BestFrontendScore ||
                (request.FrontendScore == currentBestResult.BestFrontendScore && attemptTime < currentBestResult.FrontendAchievedAt))
            {
                currentBestResult.BestFrontendScore = request.FrontendScore;
                currentBestResult.FrontendAchievedAt = attemptTime;
                updated = true;
            }

            if (request.BackendScore > currentBestResult.BestBackendScore ||
                (request.BackendScore == currentBestResult.BestBackendScore && attemptTime < currentBestResult.BackendAchievedAt))
            {
                currentBestResult.BestBackendScore = request.BackendScore;
                currentBestResult.BackendAchievedAt = attemptTime;
                updated = true;
            }

            if (request.MobdevScore > currentBestResult.BestMobdevScore ||
                (request.MobdevScore == currentBestResult.BestMobdevScore && attemptTime < currentBestResult.MobdevAchievedAt))
            {
                currentBestResult.BestMobdevScore = request.MobdevScore;
                currentBestResult.MobdevAchievedAt = attemptTime;
                updated = true;
            }

            if (updated)
            {
                currentBestResult.UpdatedAt = attemptTime;
            }

            bestResult = currentBestResult;
        }

        await _context.SaveChangesAsync(cancellationToken);

        var resultDto = new BestResultDto
        {
            BestFrontendScore = bestResult.BestFrontendScore,
            BestBackendScore = bestResult.BestBackendScore,
            BestMobdevScore = bestResult.BestMobdevScore,
            FrontendAchievedAt = bestResult.FrontendAchievedAt,
            BackendAchievedAt = bestResult.BackendAchievedAt,
            MobdevAchievedAt = bestResult.MobdevAchievedAt
        };

        return new Response<BestResultDto>(resultDto);
    }
}
