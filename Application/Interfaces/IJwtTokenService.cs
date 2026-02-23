using Domain.Entities;

namespace Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateAccessToken(ApplicationUser user, IList<string> roles);
    string GenerateRefreshToken();
    DateTime GetRefreshTokenExpirationDate();
}
