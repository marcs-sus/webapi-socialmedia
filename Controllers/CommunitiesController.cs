using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Data;
using WebApiSocialMedia.DTOs.Communities;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommunitiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommunitiesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CommunityResponseDto>>> GetAll()
    {
        var communities = await _context.Communities
            .Select(c => new CommunityResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                OwnerId = c.OwnerId,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(communities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityResponseDto>> GetById(int id)
    {
        var community = await _context.Communities
            .Where(c => c.Id == id)
            .Select(c => new CommunityResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                OwnerId = c.OwnerId,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();

        if (community == null)
        {
            return NotFound();
        }

        return Ok(community);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommunityResponseDto>> Create(CreateCommunityDto dto)
    {
        var exists = await _context.Communities
            .AnyAsync(c => c.Name == dto.Name);

        if (exists)
        {
            return BadRequest("Community already exists.");
        }

        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var ownerExists = await _context.Users
            .AnyAsync(u => u.Id == userId);

        if (!ownerExists)
        {
            return BadRequest("Owner does not exist.");
        }

        var community = new Community
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = userId
        };

        _context.Communities.Add(community);

        await _context.SaveChangesAsync();

        var response = new CommunityResponseDto
        {
            Id = community.Id,
            Name = community.Name,
            Description = community.Description,
            OwnerId = community.OwnerId,
            CreatedAt = community.CreatedAt
        };

        return CreatedAtAction(
            nameof(GetById),
            new { id = community.Id },
            response
        );
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCommunityDto dto)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var community = await _context.Communities
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.OwnerId == userId);

        if (community == null)
        {
            return NotFound();
        }

        var exists = await _context.Communities
            .AnyAsync(c => c.Name == dto.Name);

        if (exists)
        {
            return BadRequest("Community already exists.");
        }

        community.Name = dto.Name;
        community.Description = dto.Description;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var community = await _context.Communities
            .FirstOrDefaultAsync(c =>
                c.Id == id
                && c.OwnerId == userId);

        if (community == null)
        {
            return NotFound();
        }

        _context.Communities.Remove(community);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}