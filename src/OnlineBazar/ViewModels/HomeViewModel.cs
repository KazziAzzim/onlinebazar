using OnlineBazar.DTOs;

namespace OnlineBazar.ViewModels;

public class HomeViewModel
{
    public IEnumerable<ProductDto> FeaturedProducts { get; set; } = new List<ProductDto>();
    public IEnumerable<BannerDto> Banners { get; set; } = new List<BannerDto>();
    public IEnumerable<ProductDto> CarouselProducts { get; set; } = new List<ProductDto>();
    public IEnumerable<CategoryProductGroupViewModel> CategoryGroups { get; set; } = new List<CategoryProductGroupViewModel>();
}

public class CategoryProductGroupViewModel
{
    public string CategoryName { get; set; } = string.Empty;
    public string CategorySlug { get; set; } = string.Empty;
    public IEnumerable<ProductDto> Products { get; set; } = new List<ProductDto>();
}
