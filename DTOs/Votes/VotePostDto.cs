using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Votes;

public class VotePostDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [Range(-1, 1)]
    public short VoteType { get; set; }
}