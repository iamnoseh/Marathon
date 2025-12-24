using Application.DTOs;
using Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Marathon.Commands.SubmitAttempt;

public class SubmitMarathonAttemptCommand : IRequest<Response<BestResultDto>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    
    public int FrontendScore { get; set; }
    public int BackendScore { get; set; }
}
