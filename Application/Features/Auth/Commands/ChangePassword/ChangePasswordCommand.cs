using Application.Responses;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Response<bool>>
{
    [JsonIgnore]
    public string UserId { get; set; } = string.Empty;
    
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
