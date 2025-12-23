using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Marathon.Queries.GetMyRank;

public class GetMyRankQuery : IRequest<Response<int>>
{
    public string UserId { get; set; } = string.Empty;
}
