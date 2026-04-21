using OnlineBazar.ViewModels;

namespace OnlineBazar.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetOverviewAsync();
}
