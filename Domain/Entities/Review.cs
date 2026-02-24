using Domain.Common;

namespace Domain.Entities;

public class Review : BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public bool IsApproved { get; set; } = false;
    
    public ApplicationUser User { get; set; } = null!;
}
