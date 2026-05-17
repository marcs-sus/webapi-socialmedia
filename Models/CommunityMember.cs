using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSocialMedia.Models;

public class CommunityMember
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public int CommunityId { get; set; }

    [ForeignKey(nameof(CommunityId))]
    public Community Community { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}