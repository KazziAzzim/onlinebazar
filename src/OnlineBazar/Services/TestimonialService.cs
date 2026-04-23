using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Services;

public class TestimonialService : ITestimonialService
{
    private readonly ApplicationDbContext _dbContext;

    public TestimonialService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TestimonialDto>> GetTestimonialsAsync()
    {
        return await _dbContext.Testimonials
            .AsNoTracking()
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.Id)
            .Select(t => new TestimonialDto
            {
                Id = t.Id,
                CustomerName = t.CustomerName,
                City = t.City,
                ReviewText = t.ReviewText,
                Rating = t.Rating
            })
            .ToListAsync();
    }
}
