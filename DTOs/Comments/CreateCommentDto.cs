using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Comments;

public class CreateCommentDto
{
    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [Required]
    public int PostId { get; set; }

    // Optional reply target

    public int? ParentCommentId { get; set; }
}