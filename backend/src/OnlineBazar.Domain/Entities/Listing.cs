using OnlineBazar.Domain.Enums;

namespace OnlineBazar.Domain.Entities;

public class Listing : BaseEntity
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public decimal? Price { get; set; }
    public string CurrencyCode { get; set; } = "BDT";
    public string? Location { get; set; }
    public ListingStatus Status { get; set; } = ListingStatus.Draft;
    public DateTime? PublishedDate { get; set; }
}
