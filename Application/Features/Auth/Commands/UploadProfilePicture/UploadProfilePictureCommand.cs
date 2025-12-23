using Application.Responses;
using MediatR;

namespace Application.Features.Auth.Commands.UploadProfilePicture;

public class UploadProfilePictureCommand : IRequest<Response<string>>
{
    public string UserId { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}
