using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Response<UserProfileDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;

    public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Response<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            return new Response<UserProfileDto>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        var bestResult = await _context.BestResults
            .FirstOrDefaultAsync(br => br.UserId == request.UserId && !br.IsDeleted, cancellationToken);

        var profile = new UserProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email ?? string.Empty,
            ProfilePicture = user.ProfilePicture,
            CreatedAt = user.CreatedAt,
            BestResult = bestResult != null ? new BestResultDto
            {
                BestFrontendScore = bestResult.BestFrontendScore,
                BestBackendScore = bestResult.BestBackendScore,
                FrontendAchievedAt = bestResult.FrontendAchievedAt,
                BackendAchievedAt = bestResult.BackendAchievedAt
            } : null
        };

        return new Response<UserProfileDto>(profile);
    }
}
