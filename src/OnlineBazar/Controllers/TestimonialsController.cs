using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestimonialsController : ControllerBase
{
    private readonly ITestimonialService _testimonialService;

    public TestimonialsController(ITestimonialService testimonialService)
    {
        _testimonialService = testimonialService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTestimonials()
    {
        var testimonials = await _testimonialService.GetTestimonialsAsync();
        return Ok(testimonials);
    }
}
