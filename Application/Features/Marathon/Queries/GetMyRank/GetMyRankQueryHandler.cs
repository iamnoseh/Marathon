using System.Net;
using Application.Constants;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Queries.GetMyRank;

public class GetMyRankQueryHandler : IRequestHandler<GetMyRankQuery, Response<int>>
{
    private readonly IApplicationDbContext _context;

    public GetMyRankQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<int>> Handle(GetMyRankQuery request, CancellationToken cancellationToken)
    {
        var userBestResult = await _context.BestResults
            .FirstOrDefaultAsync(br => br.UserId == request.UserId && !br.IsDeleted, cancellationToken);

        if (userBestResult == null)
        {
            return new Response<int>(HttpStatusCode.NotFound, Messages.Marathon.ResultNotFound);
        }

        var userTotalScore = userBestResult.BestFrontendScore + userBestResult.BestBackendScore;
        var userEarliestTime = userBestResult.FrontendAchievedAt < userBestResult.BackendAchievedAt 
            ? userBestResult.FrontendAchievedAt 
            : userBestResult.BackendAchievedAt;

        var rank = await _context.BestResults
            .Where(br => !br.IsDeleted)
            .Where(br =>
                (br.BestFrontendScore + br.BestBackendScore) > userTotalScore ||
                ((br.BestFrontendScore + br.BestBackendScore) == userTotalScore &&
                 (br.FrontendAchievedAt < br.BackendAchievedAt ? br.FrontendAchievedAt : br.BackendAchievedAt) < userEarliestTime))
            .CountAsync(cancellationToken);

        var userRank = rank + 1;

        return new Response<int>(userRank);
    }
}
