using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Queries.GetLeaderboard;

public class GetLeaderboardQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetLeaderboardQuery, Response<List<LeaderboardEntryDto>>>
{
    public async Task<Response<List<LeaderboardEntryDto>>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var leaderboard = await context.BestResults
            .Include(br => br.User)
            .Where(br => !br.IsDeleted)
            .OrderByDescending(br => br.Score)
            .ThenBy(br => br.AchievedAt)
            .Take(5)
            .Select(br => new LeaderboardEntryDto
            {
                FullName = br.User.FullName,
                Score = br.Score,
                AchievedAt = br.AchievedAt
            })
            .ToListAsync(cancellationToken);

        for (int i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = i + 1;
        }

        return new Response<List<LeaderboardEntryDto>>(leaderboard);
    }
}
