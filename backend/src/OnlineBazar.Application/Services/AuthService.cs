using Microsoft.EntityFrameworkCore;
using OnlineBazar.Application.DTOs.Auth;
using OnlineBazar.Application.Interfaces;

namespace OnlineBazar.Application.Services;

public class AuthService(IUnitOfWork uow, IJwtTokenGenerator tokenGenerator) : IAuthService
{
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken ct = default)
    {
        var user = await uow.Users.Query()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == request.Email && !x.IsDeleted, ct)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        // Replace with ASP.NET Identity PasswordHasher in full implementation.
        if (user.PasswordHash != request.Password)
            throw new UnauthorizedAccessException("Invalid credentials");

        return new AuthResponseDto
        {
            AccessToken = tokenGenerator.Generate(user),
            ExpiresAtUtc = DateTime.UtcNow.AddHours(2),
            FullName = user.FullName,
            Roles = user.Roles.Select(r => r.Name).ToArray()
        };
    }
}
