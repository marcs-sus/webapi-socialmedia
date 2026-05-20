using System.ComponentModel.DataAnnotations;

namespace WebApiSocialMedia.DTOs.Users;

public class UpdateUserDto
{
    [Required]
    [StringLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
}