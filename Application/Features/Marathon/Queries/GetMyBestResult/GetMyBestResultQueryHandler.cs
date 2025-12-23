using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Queries.GetMyBestResult;

public class GetMyBestResultQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMyBestResultQuery, Response<BestResultDto>>
{
    public async Task<Response<BestResultDto>> Handle(GetMyBestResultQuery request, CancellationToken cancellationToken)
    {
        var bestResult = await context.BestResults
            .Where(br => br.UserId == request.UserId && !br.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (bestResult == null)
        {
            return new Response<BestResultDto>(HttpStatusCode.NotFound, Messages.Marathon.ResultNotFound);
        }

        var dto = new BestResultDto
        {
            Score = bestResult.Score,
            AchievedAt = bestResult.AchievedAt
        };

        return new Response<BestResultDto>(dto);
    }
}
