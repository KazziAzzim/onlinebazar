using Microsoft.AspNetCore.Mvc;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
public class CustomersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
