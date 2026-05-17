namespace WebApiSocialMedia.DTOs.Communities;

public class CommunityResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int OwnerId { get; set; }

    public DateTime CreatedAt { get; set; }
}