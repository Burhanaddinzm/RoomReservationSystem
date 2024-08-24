using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;
using RoomReservationSystem.ViewModels.Auth;

namespace RoomReservationSystem.Controllers;

public class AuthController : Controller
{
    private readonly IUserService _userService;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthController(
        IUserService userService,
        SignInManager<AppUser> signInManager)
    {
        _userService = userService;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        var user = await _userService.GetUserAsync(loginVM.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid Credentials");
            return View(loginVM);
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut) ModelState.AddModelError("", "You have been locked out, try again in 1 minutes!");
            else ModelState.AddModelError("", "Invalid Credentials!");

            return View(loginVM);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            return View(registerVM);
        }

        if (await _userService.CheckDuplicate(registerVM.Email))
        {
            ModelState.AddModelError("", "User with this email already exists");
            return View(registerVM);
        }

        var result = await _userService.CreateAsync(registerVM);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerVM);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
