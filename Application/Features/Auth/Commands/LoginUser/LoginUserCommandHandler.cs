using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IApplicationDbContext _context;

    public LoginUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    public async Task<Response<AuthResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLowerInvariant();

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.Unauthorized, Messages.Auth.InvalidCredentials);
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.Unauthorized, Messages.Auth.InvalidCredentials);
        }

        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiration = _jwtTokenService.GetRefreshTokenExpirationDate();

        var refreshTokenEntity = new Domain.Entities.RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = refreshTokenExpiration,
            IsRevoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var authResponse = new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = refreshTokenExpiration
        };

        return new Response<AuthResponseDto>(authResponse);
    }
}
