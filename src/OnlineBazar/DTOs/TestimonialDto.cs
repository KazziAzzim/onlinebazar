namespace OnlineBazar.DTOs;

public class TestimonialDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ReviewText { get; set; } = string.Empty;
    public int Rating { get; set; }
}
