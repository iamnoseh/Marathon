using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<MarathonAttempt> MarathonAttempts { get; }
    DbSet<BestResult> BestResults { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Review> Reviews { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
