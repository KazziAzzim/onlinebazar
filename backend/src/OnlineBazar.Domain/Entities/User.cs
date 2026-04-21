namespace OnlineBazar.Domain.Entities;

public class User : BaseEntity
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}
