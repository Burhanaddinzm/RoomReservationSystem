using Microsoft.AspNetCore.Identity;

namespace RoomReservationSystem.Models;

public class AppUser : IdentityUser
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string FullName => $"{Name} {Surname}";
    public virtual ICollection<Reservation> HostedReservations { get; set; }
    public virtual ICollection<Reservation> Reservations { get; set; }
    public AppUser()
    {
        HostedReservations = new List<Reservation>();
        Reservations = new List<Reservation>();
    }
}
