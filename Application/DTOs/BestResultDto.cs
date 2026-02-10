namespace Application.DTOs;

public class BestResultDto
{
    public int BestFrontendScore { get; set; }
    public int BestBackendScore { get; set; }
    public int BestMobdevScore { get; set; }
    public DateTime FrontendAchievedAt { get; set; }
    public DateTime BackendAchievedAt { get; set; }
    public DateTime MobdevAchievedAt { get; set; }
}
