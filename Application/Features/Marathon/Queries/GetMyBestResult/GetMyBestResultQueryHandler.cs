using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Marathon.Queries.GetMyBestResult;

public class GetMyBestResultQueryHandler : IRequestHandler<GetMyBestResultQuery, Response<BestResultDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMyBestResultQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<BestResultDto>> Handle(GetMyBestResultQuery request, CancellationToken cancellationToken)
    {
        var bestResult = await _context.BestResults
            .Where(br => br.UserId == request.UserId && !br.IsDeleted)
            .FirstOrDefaultAsync(cancellationToken);

        if (bestResult == null)
        {
            return new Response<BestResultDto>(HttpStatusCode.NotFound, Messages.Marathon.ResultNotFound);
        }

        var dto = new BestResultDto
        {
            BestFrontendScore = bestResult.BestFrontendScore,
            BestBackendScore = bestResult.BestBackendScore,
            FrontendAchievedAt = bestResult.FrontendAchievedAt,
            BackendAchievedAt = bestResult.BackendAchievedAt
        };

        return new Response<BestResultDto>(dto);
    }
}
