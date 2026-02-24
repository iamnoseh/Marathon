using Application.Responses;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Features.Reviews.Commands.AddReview;

public class AddReviewCommand : IRequest<Response<int>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Бахо бояд аз 1 то 5 бошад.")]
    public int Rating { get; set; } = 5;
}
