namespace OnlineBazar.Application.DTOs.Listing;

public class ListingCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal? Price { get; set; }
    public string? Location { get; set; }
}

public class ListingReadDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? Price { get; set; }
    public string? Location { get; set; }
    public string Status { get; set; } = string.Empty;
}
