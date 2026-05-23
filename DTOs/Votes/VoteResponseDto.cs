namespace WebApiSocialMedia.DTOs.Votes;

public class VoteResponseDto
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public short VoteType { get; set; }

    public int TotalVotes { get; set; }
}