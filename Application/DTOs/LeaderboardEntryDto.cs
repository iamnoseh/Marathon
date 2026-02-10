namespace Application.DTOs;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public int FrontendScore { get; set; }
    public int BackendScore { get; set; }
    public int MobdevScore { get; set; }
    public int TotalScore { get; set; }
    public DateTime LastAchievedAt { get; set; }
}
