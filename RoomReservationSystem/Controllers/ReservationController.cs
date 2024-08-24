using Microsoft.AspNetCore.Mvc;

namespace RoomReservationSystem.Controllers;

public class ReservationController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? roomId)
    {
        return View();
    }
}
