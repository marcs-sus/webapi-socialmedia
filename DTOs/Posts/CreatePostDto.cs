using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Posts;

public class CreatePostDto
{
    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public int CommunityId { get; set; }
}