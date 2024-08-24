﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoomReservationSystem.ViewModels.Auth;

public class RegisterVM
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [DisplayName("Confirm Password")]
    public string ConfirmPassword { get; set; } = null!;
}
