using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpirationDate();
}
