using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;
using RoomReservationSystem.ViewModels.Reservation;

namespace RoomReservationSystem.Controllers;
[Authorize]
public class ReservationController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRoomService _roomService;
    private readonly IUserService _userService;
    private readonly IReservationService _reservationService;
    public ReservationController(
        UserManager<AppUser> userManager,
        IRoomService roomService,
        IUserService userService,
        IReservationService reservationService)
    {
        _userManager = userManager;
        _roomService = roomService;
        _userService = userService;
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reservations = await _reservationService.GetAllReservationsAsync();
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? roomId)
    {
        if (roomId == null)
        {
            return BadRequest();
        }

        var room = await _roomService.GetRoomAsync(roomId.Value);
        if (room == null)
        {
            return NotFound($"Room with id-{roomId} does not exist");
        }
        if (!room.IsAvailable)
        {
            return BadRequest();
        }

        var currentUser = await _userService.FindCurrentUserAsync();

        ViewBag.Users = await _userManager.Users.Where(x => x.Id != currentUser!.Id).ToListAsync();
        ViewBag.Room = room;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateReservationVM reservationVM)
    {
        var currentUser = await _userService.FindCurrentUserAsync();

        if (!ModelState.IsValid)
        {
            ViewBag.Users = await _userManager.Users.Where(x => x.Id != currentUser!.Id).ToListAsync();
            return View(reservationVM);
        }

        await _reservationService.CreateAsync(reservationVM, currentUser!);
        return RedirectToAction(nameof(Index));
    }

}
