using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiSocialMedia.Data;
using WebApiSocialMedia.DTOs.Comments;
using WebApiSocialMedia.Models;

namespace WebApiSocialMedia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CommentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("post/{postId}")]
    public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetByPost(int postId)
    {
        var postExists = await _context.Posts
            .AnyAsync(p => p.Id == postId);

        if (!postExists)
        {
            return NotFound("Post not found.");
        }

        var comments = await _context.Comments
            .Include(c => c.Author)
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentResponseDto
            {
                Id = c.Id,
                Content = c.Content,
                AuthorId = c.AuthorId,
                AuthorUsername = c.Author.Username,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return Ok(comments);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CommentResponseDto>> Create(CreateCommentDto dto)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var authorExists = await _context.Users
            .AnyAsync(u => u.Id == userId);

        if (!authorExists)
        {
            return BadRequest("Author does not exist.");
        }

        var postExists = await _context.Posts
            .AnyAsync(p => p.Id == dto.PostId);

        if (!postExists)
        {
            return BadRequest("Post does not exist.");
        }

        if (dto.ParentCommentId.HasValue)
        {
            var parentExists = await _context.Comments
                .AnyAsync(c => c.Id == dto.ParentCommentId.Value);

            if (!parentExists)
            {
                return BadRequest("Parent comment does not exist.");
            }
        }

        var comment = new Comment
        {
            Content = dto.Content,
            AuthorId = userId,
            PostId = dto.PostId,
            ParentCommentId = dto.ParentCommentId
        };

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        var createdComment = await _context.Comments
            .Include(c => c.Author)
            .Where(c => c.Id == comment.Id)
            .Select(c => new CommentResponseDto
            {
                Id = c.Id,
                Content = c.Content,
                AuthorId = c.AuthorId,
                AuthorUsername = c.Author.Username,
                PostId = c.PostId,
                ParentCommentId = c.ParentCommentId,
                CreatedAt = c.CreatedAt
            })
            .FirstAsync();

        return Ok(createdComment);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var comment = await _context.Comments
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.AuthorId == userId);

        if (comment == null)
        {
            return NotFound();
        }

        _context.Comments.Remove(comment);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}