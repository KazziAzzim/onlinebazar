namespace OnlineBazar.ViewModels;

public class OrderSummaryViewModel
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}
