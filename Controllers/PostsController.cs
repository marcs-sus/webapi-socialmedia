using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Data;
using WebApiSocialMedia.DTOs.Posts;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PostsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetAll()
    {
        var posts = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Community)
            .Include(p => p.Votes)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                AuthorId = p.AuthorId,
                AuthorUsername = p.Author.Username,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                VoteCount = p.Votes.Sum(v => v.VoteType),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostResponseDto>> GetById(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Community)
            .Include(p => p.Votes)
            .Where(p => p.Id == id)
            .Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                AuthorId = p.AuthorId,
                AuthorUsername = p.Author.Username,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                VoteCount = p.Votes.Sum(v => v.VoteType),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return NotFound();
        }

        return Ok(post);
    }

    [HttpGet("community/{communityId}")]
    public async Task<ActionResult<IEnumerable<PostResponseDto>>> GetByCommunity(int communityId)
    {
        var communityExists = await _context.Communities
            .AnyAsync(c => c.Id == communityId);

        if (!communityExists)
        {
            return NotFound("Community not found.");
        }

        var posts = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Community)
            .Include(p => p.Votes)
            .Where(p => p.CommunityId == communityId)
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                AuthorId = p.AuthorId,
                AuthorUsername = p.Author.Username,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                VoteCount = p.Votes.Sum(v => v.VoteType),
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .ToListAsync();

        return Ok(posts);
    }

    [HttpPost]
    public async Task<ActionResult<PostResponseDto>> Create(CreatePostDto dto)
    {
        var authorExists = await _context.Users
            .AnyAsync(u => u.Id == dto.AuthorId);

        if (!authorExists)
        {
            return BadRequest("Author does not exist.");
        }

        var communityExists = await _context.Communities
            .AnyAsync(c => c.Id == dto.CommunityId);

        if (!communityExists)
        {
            return BadRequest("Community does not exist.");
        }

        var post = new Post
        {
            Title = dto.Title,
            Content = dto.Content,
            AuthorId = dto.AuthorId,
            CommunityId = dto.CommunityId
        };

        _context.Posts.Add(post);

        await _context.SaveChangesAsync();

        var createdPost = await _context.Posts
            .Include(p => p.Author)
            .Include(p => p.Community)
            .Where(p => p.Id == post.Id)
            .Select(p => new PostResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                AuthorId = p.AuthorId,
                AuthorUsername = p.Author.Username,
                CommunityId = p.CommunityId,
                CommunityName = p.Community.Name,
                VoteCount = 0,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            })
            .FirstAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = post.Id },
            createdPost
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePostDto dto)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        post.Title = dto.Title;
        post.Content = dto.Content;
        post.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}