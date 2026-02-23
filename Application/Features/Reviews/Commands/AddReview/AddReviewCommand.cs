using Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommand : IRequest<Response<int>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}
