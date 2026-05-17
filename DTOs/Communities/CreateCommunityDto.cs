using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Communities;

public class CreateCommunityDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public int OwnerId { get; set; }
}