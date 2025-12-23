using System.Net;
using Application.Constants;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Queries.GetMyRank;

public class GetMyRankQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMyRankQuery, Response<int>>
{
    public async Task<Response<int>> Handle(GetMyRankQuery request, CancellationToken cancellationToken)
    {
        var userBestResult = await context.BestResults
            .FirstOrDefaultAsync(br => br.UserId == request.UserId && !br.IsDeleted, cancellationToken);

        if (userBestResult == null)
        {
            return new Response<int>(HttpStatusCode.NotFound, Messages.Marathon.ResultNotFound);
        }

        var rank = await context.BestResults
            .Where(br => !br.IsDeleted)
            .Where(br => 
                br.Score > userBestResult.Score ||
                (br.Score == userBestResult.Score && br.AchievedAt < userBestResult.AchievedAt))
            .CountAsync(cancellationToken);

        var userRank = rank + 1;

        return new Response<int>(userRank);
    }
}
