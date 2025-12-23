using Application.DTOs;
using Application.Responses;
using MediatR;

namespace Application.Features.Marathon.Queries.GetMyBestResult;

public class GetMyBestResultQuery : IRequest<Response<BestResultDto>>
{
    public string UserId { get; set; } = string.Empty;
}
