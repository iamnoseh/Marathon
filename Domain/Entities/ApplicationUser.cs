using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<MarathonAttempt> Attempts { get; set; } = new List<MarathonAttempt>();
    public BestResult? BestResult { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
