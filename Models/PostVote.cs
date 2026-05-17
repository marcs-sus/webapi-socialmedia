using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSocialMedia.Models;

public class PostVote
{
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    public int PostId { get; set; }

    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; } = null!;

    // +1 = upvote
    // -1 = downvote

    public short VoteType { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}