using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Response<AuthResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService)
    {
        _context = context;
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Response<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshTokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsDeleted, cancellationToken);

        if (refreshTokenEntity == null)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.Unauthorized, Messages.Auth.RefreshTokenNotFound);
        }

        if (refreshTokenEntity.IsRevoked)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.Unauthorized, Messages.Auth.RefreshTokenRevoked);
        }

        if (refreshTokenEntity.ExpiresAt < DateTime.UtcNow)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.Unauthorized, Messages.Auth.RefreshTokenExpired);
        }

        var user = await _userManager.FindByIdAsync(refreshTokenEntity.UserId);
        if (user == null)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.NotFound, Messages.Auth.UserNotFound);
        }

        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var newRefreshTokenExpiration = _jwtTokenService.GetRefreshTokenExpirationDate();

        refreshTokenEntity.IsRevoked = true;
        refreshTokenEntity.UpdatedAt = DateTime.UtcNow;

        var newRefreshTokenEntity = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = newRefreshToken,
            ExpiresAt = newRefreshTokenExpiration,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.RefreshTokens.AddAsync(newRefreshTokenEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var authResponse = new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = newRefreshTokenExpiration
        };

        return new Response<AuthResponseDto>(authResponse);
    }
}
