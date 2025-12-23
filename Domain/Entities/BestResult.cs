using Domain.Common;

namespace Domain.Entities;

public class BestResult : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime AchievedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
