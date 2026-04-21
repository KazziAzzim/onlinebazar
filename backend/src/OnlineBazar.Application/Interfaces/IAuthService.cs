using OnlineBazar.Application.DTOs.Auth;

namespace OnlineBazar.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default);
}
