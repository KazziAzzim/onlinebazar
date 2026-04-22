using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;

    public CategoryService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = (await _uow.Categories.GetAllAsync(c => !c.IsDeleted)).Cast<Category>();
        return categories.OrderBy(c => c.Name).Select(MapToDto);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _uow.Categories.GetByIdAsync(id) as Category;
        return category is null || category.IsDeleted ? null : MapToDto(category);
    }

    public async Task CreateAsync(CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description
        };

        await _uow.Categories.AddAsync(category);
        await _uow.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryDto dto)
    {
        var category = await _uow.Categories.GetByIdAsync(dto.Id) as Category;
        if (category is null) throw new KeyNotFoundException("Category not found.");

        category.Name = dto.Name;
        category.Slug = dto.Slug;
        category.Description = dto.Description;
        category.ModifiedDate = DateTime.UtcNow;

        _uow.Categories.Update(category);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _uow.Categories.GetByIdAsync(id) as Category;
        if (category is null) return;

        category.IsDeleted = true;
        category.ModifiedDate = DateTime.UtcNow;
        _uow.Categories.Update(category);
        await _uow.SaveChangesAsync();
    }

    private static CategoryDto MapToDto(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Slug = category.Slug,
        Description = category.Description
    };
}
