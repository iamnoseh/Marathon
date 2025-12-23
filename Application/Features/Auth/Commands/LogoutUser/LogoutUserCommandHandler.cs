using System.Net;
using Application.Constants;
using Application.Responses;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.LogoutUser;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Response<bool>>
{
    private readonly IApplicationDbContext _context;

    public LogoutUserCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Response<bool>> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken && !rt.IsDeleted, cancellationToken);

        if (refreshToken == null)
        {
            return new Response<bool>(HttpStatusCode.NotFound, Messages.Auth.RefreshTokenNotFound);
        }

        refreshToken.IsRevoked = true;
        refreshToken.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new Response<bool>(true);
    }
}
