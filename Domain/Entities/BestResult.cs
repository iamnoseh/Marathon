using Domain.Common;

namespace Domain.Entities;

public class BestResult : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public int BestFrontendScore { get; set; }
    public int BestBackendScore { get; set; }
    public int BestMobdevScore { get; set; }
    public DateTime FrontendAchievedAt { get; set; }
    public DateTime BackendAchievedAt { get; set; }
    public DateTime MobdevAchievedAt { get; set; }
    public ApplicationUser User { get; set; } = null!;
}
