using Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<Response<bool>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string? ProfilePicturePath { get; set; }
    
    public string? FullName { get; set; }
}
