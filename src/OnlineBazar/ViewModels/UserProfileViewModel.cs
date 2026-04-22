using System.ComponentModel.DataAnnotations;

namespace OnlineBazar.ViewModels;

public class UserProfileViewModel
{
    [Required]
    [StringLength(120)]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
