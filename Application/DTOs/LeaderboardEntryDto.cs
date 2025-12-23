namespace Application.DTOs;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    
    public string FullName { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime AchievedAt { get; set; }
}
