using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
{
    public DbSet<MarathonAttempt> MarathonAttempts { get; set; }
    public DbSet<BestResult> BestResults { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.HasMany(u => u.Attempts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(u => u.BestResult)
                .WithOne(b => b.User)
                .HasForeignKey<BestResult>(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<BestResult>(entity =>
        {
            entity.HasIndex(b => new { TotalScore = b.BestFrontendScore + b.BestBackendScore, b.FrontendAchievedAt, b.BackendAchievedAt });

            entity.HasQueryFilter(b => !b.IsDeleted);
        });

        builder.Entity<MarathonAttempt>(entity =>
        {
            entity.HasIndex(a => a.UserId);
            entity.HasIndex(a => a.AchievedAt);

            entity.HasQueryFilter(a => !a.IsDeleted);
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasIndex(r => r.Token).IsUnique();
            entity.HasIndex(r => r.UserId);

            entity.HasQueryFilter(r => !r.IsDeleted);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            if (entry.Entity is Domain.Common.BaseEntity entity)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
