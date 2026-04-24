using System.ComponentModel.DataAnnotations;

namespace OnlineBazar.DTOs;

public class DealDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(180)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    [StringLength(20)]
    public string DiscountType { get; set; } = "Flat";

    [Range(typeof(decimal), "0.01", "9999999")]
    public decimal DiscountValue { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; } = DateTime.UtcNow;

    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(1);

    [StringLength(500)]
    public string? RedirectUrl { get; set; }

    [Range(0, int.MaxValue)]
    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; } = true;
}
