using Domain.Common;

namespace Domain.Entities;

public class MarathonAttempt : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime AchievedAt { get; set; } = DateTime.UtcNow;
    public ApplicationUser User { get; set; } = null!;
}
