using Microsoft.AspNetCore.Mvc;

namespace RoomReservationSystem.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
