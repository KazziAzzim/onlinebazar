using OnlineBazar.DTOs;

namespace OnlineBazar.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductDto> FeaturedProducts { get; set; } = new List<ProductDto>();
}
