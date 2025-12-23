namespace Application.DTOs;

public class UserProfileDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public DateTime CreatedAt { get; set; }
    public BestResultDto? BestResult { get; set; }
}
