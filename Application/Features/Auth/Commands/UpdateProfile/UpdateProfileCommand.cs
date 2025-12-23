using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommand : IRequest<Response<bool>>
{
    public string UserId { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? ProfilePicture { get; set; }
}
