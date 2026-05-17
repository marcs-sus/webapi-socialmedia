using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiSocialMedia.Models;

public class Post
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public int AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; set; } = null!;

    [Required]
    public int CommunityId { get; set; }

    [ForeignKey(nameof(CommunityId))]
    public Community Community { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<PostVote> Votes { get; set; } = new List<PostVote>();
}