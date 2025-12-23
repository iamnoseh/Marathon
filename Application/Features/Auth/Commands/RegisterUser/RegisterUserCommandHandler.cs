using System.Net;
using Application.Constants;
using Application.DTOs;
using Application.Interfaces;
using Application.Responses;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<AuthResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IApplicationDbContext _context;

    public RegisterUserCommandHandler(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        IApplicationDbContext context)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _context = context;
    }

    public async Task<Response<AuthResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLowerInvariant();

        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return new Response<AuthResponseDto>(HttpStatusCode.BadRequest, Messages.Auth.UserAlreadyExists);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = request.FullName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Response<AuthResponseDto>(HttpStatusCode.BadRequest, $"{Messages.Auth.RegistrationFailed}: {errors}");
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
