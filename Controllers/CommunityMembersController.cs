using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Data;
using WebApiSocialMedia.DTOs.CommunityMembers;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunityMembersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommunityMembersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<CommunityMemberResponseDto>> Join(
    JoinCommunityDto dto)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == dto.UserId);

        if (!userExists)
        {
            return BadRequest("User not found.");
        }

        var communityExists = await _context.Communities
            .AnyAsync(c => c.Id == dto.CommunityId);

        if (!communityExists)
        {
            return BadRequest("Community not found.");
        }

        var alreadyMember = await _context.CommunityMembers
            .AnyAsync(cm =>
                cm.UserId == dto.UserId &&
                cm.CommunityId == dto.CommunityId);

        if (alreadyMember)
        {
            return BadRequest("User is already a member.");
        }

        var membership = new CommunityMember
        {
            UserId = dto.UserId,
            CommunityId = dto.CommunityId
        };

        _context.CommunityMembers.Add(membership);

        await _context.SaveChangesAsync();

        var response = await _context.CommunityMembers
            .Include(cm => cm.User)
            .Include(cm => cm.Community)
            .Where(cm =>
                cm.UserId == dto.UserId &&
                cm.CommunityId == dto.CommunityId)
            .Select(cm => new CommunityMemberResponseDto
            {
                UserId = cm.UserId,
                Username = cm.User.Username,
                CommunityId = cm.CommunityId,
                CommunityName = cm.Community.Name,
                JoinedAt = cm.JoinedAt
            })
            .FirstAsync();

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Leave(
        [FromQuery] int userId,
        [FromQuery] int communityId)
    {
        var membership = await _context.CommunityMembers
            .FirstOrDefaultAsync(cm =>
                cm.UserId == userId &&
                cm.CommunityId == communityId);

        if (membership == null)
        {
            return NotFound("Membership not found.");
        }

        _context.CommunityMembers.Remove(membership);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("community/{communityId}")]
    public async Task<ActionResult<IEnumerable<CommunityMemberResponseDto>>> GetMembers(
        int communityId)
    {
        var communityExists = await _context.Communities
            .AnyAsync(c => c.Id == communityId);

        if (!communityExists)
        {
            return NotFound("Community not found.");
        }

        var members = await _context.CommunityMembers
            .Include(cm => cm.User)
            .Include(cm => cm.Community)
            .Where(cm => cm.CommunityId == communityId)
            .Select(cm => new CommunityMemberResponseDto
            {
                UserId = cm.UserId,
                Username = cm.User.Username,
                CommunityId = cm.CommunityId,
                CommunityName = cm.Community.Name,
                JoinedAt = cm.JoinedAt
            })
            .ToListAsync();

        return Ok(members);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<CommunityMemberResponseDto>>> GetUserCommunities(
        int userId)
    {
        var userExists = await _context.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
        {
            return NotFound("User not found.");
        }

        var communities = await _context.CommunityMembers
            .Include(cm => cm.User)
            .Include(cm => cm.Community)
            .Where(cm => cm.UserId == userId)
            .Select(cm => new CommunityMemberResponseDto
            {
                UserId = cm.UserId,
                Username = cm.User.Username,
                CommunityId = cm.CommunityId,
                CommunityName = cm.Community.Name,
                JoinedAt = cm.JoinedAt
            })
            .ToListAsync();

        return Ok(communities);
    }
}