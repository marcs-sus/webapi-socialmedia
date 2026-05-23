using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Data;
using WebApiSocialMedia.DTOs.Votes;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Controllers;

[ApiController]
[Route("api/posts/{postId}/vote")]
public class VotesController : ControllerBase
{
    private readonly AppDbContext _context;

    public VotesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<VoteResponseDto>> Vote(
    int postId,
    VotePostDto dto)
    {
        var postExists = await _context.Posts
            .AnyAsync(p => p.Id == postId);

        if (!postExists)
        {
            return NotFound("Post not found.");
        }

        var userExists = await _context.Users
            .AnyAsync(u => u.Id == dto.UserId);

        if (!userExists)
        {
            return BadRequest("User not found.");
        }

        var existingVote = await _context.PostVotes
            .FirstOrDefaultAsync(v =>
                v.PostId == postId &&
                v.UserId == dto.UserId);

        if (dto.VoteType == 0)
        {
            if (existingVote != null)
            {
                _context.PostVotes.Remove(existingVote);

                await _context.SaveChangesAsync();
            }

            var totalAfterRemoval = await _context.PostVotes
                .Where(v => v.PostId == postId)
                .SumAsync(v => (int?)v.VoteType) ?? 0;

            return Ok(new VoteResponseDto
            {
                PostId = postId,
                UserId = dto.UserId,
                VoteType = 0,
                TotalVotes = totalAfterRemoval
            });
        }

        if (existingVote == null)
        {
            var vote = new PostVote
            {
                PostId = postId,
                UserId = dto.UserId,
                VoteType = dto.VoteType
            };

            _context.PostVotes.Add(vote);
        }
        else
        {
            existingVote.VoteType = dto.VoteType;
        }

        await _context.SaveChangesAsync();

        var totalVotes = await _context.PostVotes
            .Where(v => v.PostId == postId)
            .SumAsync(v => (int?)v.VoteType) ?? 0;

        return Ok(new VoteResponseDto
        {
            PostId = postId,
            UserId = dto.UserId,
            VoteType = dto.VoteType,
            TotalVotes = totalVotes
        });
    }

    [HttpGet]
    public async Task<ActionResult<int>> GetVoteCount(int postId)
    {
        var postExists = await _context.Posts
            .AnyAsync(p => p.Id == postId);

        if (!postExists)
        {
            return NotFound("Post not found.");
        }

        var totalVotes = await _context.PostVotes
            .Where(v => v.PostId == postId)
            .SumAsync(v => (int?)v.VoteType) ?? 0;

        return Ok(totalVotes);
    }
}