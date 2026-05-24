using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Votes;

public class VotePostDto
{
    [Required]
    [Range(-1, 1)]
    public short VoteType { get; set; }
}