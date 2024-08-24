﻿using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.ViewModels.Auth;

public class LoginVM
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}
