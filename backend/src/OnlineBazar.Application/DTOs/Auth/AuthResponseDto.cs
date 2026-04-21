namespace OnlineBazar.Application.DTOs.Auth;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string[] Roles { get; set; } = Array.Empty<string>();
}
