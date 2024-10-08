﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;
using RoomReservationSystem.ViewModels.Auth;

namespace RoomReservationSystem.Services.Implementations;

public class UserService : IUserService
{
    IHttpContextAccessor _accessor;
    UserManager<AppUser> _userManager;

    public UserService(
        IHttpContextAccessor accessor,
        UserManager<AppUser> userManager)
    {
        _accessor = accessor;
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(RegisterVM registerVM)
    {
        AppUser newUser = new AppUser
        {
            Name = registerVM.Name,
            Surname = registerVM.Surname,
            Email = registerVM.Email
        };
        newUser.UserName = Guid.NewGuid().ToString() + "_" + newUser.FullName.ToLower().Replace(" ", "_");

        return await _userManager.CreateAsync(newUser, registerVM.Password);
    }

    public async Task<bool> CheckDuplicate(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<AppUser?> FindCurrentUserAsync()
    {
        var userName = _accessor?.HttpContext?.User?.Identity?.Name;
        return await _userManager.FindByNameAsync(userName!);
    }

    public async Task<AppUser?> GetUserAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> IsTableEmpty()
    {
        return !await _userManager.Users.AnyAsync();
    }
}
