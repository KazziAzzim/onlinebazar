using System.ComponentModel.DataAnnotations;

namespace OnlineBazar.Areas.Admin.ViewModels;

public class UserFormViewModel
{
    public string? Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "Customer";

    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string? Password { get; set; }
}
