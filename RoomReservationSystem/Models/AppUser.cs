using Microsoft.AspNetCore.Identity;

namespace RoomReservationSystem.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FullName => $"{Name} {Surname}";

    public override string? UserName
    {
        get => base.UserName;
        set
        {
            base.UserName = Guid.NewGuid().ToString() + "_" + FullName.ToLower().Replace(' ', '_');
        }
    }
}
