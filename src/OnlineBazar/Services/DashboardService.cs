using OnlineBazar.Interfaces;
using OnlineBazar.Models;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;
    private readonly IOrderService _orderService;

    public DashboardService(IUnitOfWork uow, IOrderService orderService)
    {
        _uow = uow;
        _orderService = orderService;
    }

    public async Task<DashboardViewModel> GetOverviewAsync()
    {
        var products = await _uow.Products.GetAllAsync(p => !p.IsDeleted);
        var categories = await _uow.Categories.GetAllAsync(c => !c.IsDeleted);

        return new DashboardViewModel
        {
            ProductCount = products.Count(),
            CategoryCount = categories.Count(),
            OrdersCount = await _orderService.GetOrdersCountAsync(),
            TotalSales = await _orderService.GetTotalSalesAsync()
        };
    }
}
