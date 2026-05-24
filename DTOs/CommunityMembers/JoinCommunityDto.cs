using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.CommunityMembers;

public class JoinCommunityDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public int CommunityId { get; set; }
}