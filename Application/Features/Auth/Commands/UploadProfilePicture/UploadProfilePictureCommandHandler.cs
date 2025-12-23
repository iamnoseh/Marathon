using System.Net;
using Application.Constants;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.UploadProfilePicture;

public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, Response<string>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UploadProfilePictureCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<string>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            if (File.Exists(request.FilePath))
            {
                File.Delete(request.FilePath);
            }

            return new Response<string>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        user.ProfilePicture = request.FileUrl;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            if (File.Exists(request.FilePath))
            {
                File.Delete(request.FilePath);
            }

            return new Response<string>(HttpStatusCode.BadRequest, Messages.Auth.ProfileUpdateFailed);
        }

        return new Response<string>(request.FileUrl);
    }
}
