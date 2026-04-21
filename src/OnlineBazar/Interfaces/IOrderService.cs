namespace OnlineBazar.Interfaces;

public interface IOrderService
{
    Task<decimal> GetTotalSalesAsync();
    Task<int> GetOrdersCountAsync();
}
