using System.ComponentModel.DataAnnotations;

namespace OnlineBazar.DTOs;

public class BannerDto
{
    public int Id { get; set; }

    [StringLength(250)]
    public string? Title { get; set; }

    [StringLength(500)]
    public string? Subtitle { get; set; }

    [Required]
    [StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(500)]
    public string? RedirectUrl { get; set; }

    [Range(0, int.MaxValue)]
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
