using OnlineBazar.DTOs;

namespace OnlineBazar.Interfaces;

public interface IBannerService
{
    Task<IEnumerable<BannerDto>> GetAllAsync();
    Task<IEnumerable<BannerDto>> GetActiveBannersAsync();
    Task<BannerDto?> GetByIdAsync(int id);
    Task CreateAsync(BannerDto dto);
    Task UpdateAsync(BannerDto dto);
    Task DeleteAsync(int id);
    Task ToggleActiveAsync(int id);
}
