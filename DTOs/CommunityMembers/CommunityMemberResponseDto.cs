namespace WebApiSocialMedia.DTOs.CommunityMembers;

public class CommunityMemberResponseDto
{
    public int UserId { get; set; }

    public string Username { get; set; } = string.Empty;

    public int CommunityId { get; set; }

    public string CommunityName { get; set; } = string.Empty;

    public DateTime JoinedAt { get; set; }
}