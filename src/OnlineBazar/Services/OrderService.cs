using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;

    public OrderService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<decimal> GetTotalSalesAsync()
    {
        var orders = (await _uow.Orders.GetAllAsync(o => !o.IsDeleted)).Cast<Order>();
        return orders.Sum(o => o.TotalAmount);
    }

    public async Task<int> GetOrdersCountAsync()
    {
        var orders = await _uow.Orders.GetAllAsync(o => !o.IsDeleted);
        return orders.Count();
    }
}
