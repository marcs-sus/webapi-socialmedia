namespace WebApiSocialMedia.DTOs.Posts;

public class PostResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public string AuthorUsername { get; set; } = string.Empty;

    public int CommunityId { get; set; }

    public string CommunityName { get; set; } = string.Empty;

    public int VoteCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}