using Microsoft.AspNetCore.Identity;
using RoomReservationSystem.Models;
using RoomReservationSystem.ViewModels.Auth;

namespace RoomReservationSystem.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> Create(RegisterVM registerVM);
    Task<bool> CheckDuplicate(string email);
    Task<AppUser?> FindCurrentUserAsync();
}
