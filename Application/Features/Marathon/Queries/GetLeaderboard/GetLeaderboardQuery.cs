using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Marathon.Queries.GetLeaderboard;

public class GetLeaderboardQuery : IRequest<Response<List<LeaderboardEntryDto>>>
{
}
