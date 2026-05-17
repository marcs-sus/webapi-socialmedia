using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties

    public ICollection<Post> Posts { get; set; } = new List<Post>();

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<PostVote> Votes { get; set; } = new List<PostVote>();

    public ICollection<Community> OwnedCommunities { get; set; } = new List<Community>();

    public ICollection<CommunityMember> CommunityMembers { get; set; } = new List<CommunityMember>();
}