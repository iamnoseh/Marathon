using System.Net;
using Application.Constants;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Response<bool>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateProfileCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<bool>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            return new Response<bool>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        bool updated = false;

        if (!string.IsNullOrWhiteSpace(request.FullName) && user.FullName != request.FullName)
        {
            user.FullName = request.FullName.Trim();
            updated = true;
        }

        if (request.ProfilePicture != null && user.ProfilePicture != request.ProfilePicture)
        {
            user.ProfilePicture = request.ProfilePicture;
            updated = true;
        }

        if (updated)
        {
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new Response<bool>(HttpStatusCode.BadRequest, $"Ошибка при обновлении профиля: {errors}");
            }
        }

        return new Response<bool>(true);
    }
}
