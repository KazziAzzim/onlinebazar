using OnlineBazar.DTOs;

namespace OnlineBazar.Interfaces;

public interface ITestimonialService
{
    Task<IEnumerable<TestimonialDto>> GetTestimonialsAsync();
}
