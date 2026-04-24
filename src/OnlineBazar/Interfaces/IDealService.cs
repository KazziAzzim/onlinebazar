using OnlineBazar.DTOs;

namespace OnlineBazar.Interfaces;

public interface IDealService
{
    Task<IEnumerable<DealDto>> GetAllAsync();
    Task<IEnumerable<DealDto>> GetActiveDeals();
    Task<DealDto?> GetByIdAsync(int id);
    Task CreateAsync(DealDto dto);
    Task UpdateAsync(DealDto dto);
    Task DeleteAsync(int id);
}
