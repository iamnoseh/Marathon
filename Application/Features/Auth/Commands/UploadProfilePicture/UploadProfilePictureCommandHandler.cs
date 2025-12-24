using System.Net;
using Application.Constants;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.UploadProfilePicture;

public class UploadProfilePictureCommandHandler(UserManager<ApplicationUser> userManager)
    : IRequestHandler<UploadProfilePictureCommand, Response<string>>
{
    public async Task<Response<string>> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            if (File.Exists(request.FilePath))
            {
                File.Delete(request.FilePath);
            }

            return new Response<string>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        user.ProfilePicture = request.FileUrl;
        var result = await userManager.UpdateAsync(user);

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
