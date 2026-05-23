namespace WebApiSocialMedia.DTOs.Comments;

public class CommentResponseDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public int AuthorId { get; set; }

    public string AuthorUsername { get; set; } = string.Empty;

    public int PostId { get; set; }

    public int? ParentCommentId { get; set; }

    public DateTime CreatedAt { get; set; }
}