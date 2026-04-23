namespace OnlineBazar.Models;

public class Testimonial : BaseEntity
{
    public string CustomerName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int DisplayOrder { get; set; }
}
