using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Response<UserProfileDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Response<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            return new Response<UserProfileDto>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        var profile = new UserProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            CreatedAt = user.CreatedAt
        };

        return new Response<UserProfileDto>(profile);
    }
}
