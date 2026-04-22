using Microsoft.AspNetCore.Mvc;

namespace OnlineBazar.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}
