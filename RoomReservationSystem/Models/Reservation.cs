using RoomReservationSystem.Enums;
using RoomReservationSystem.Models.Common;

namespace RoomReservationSystem.Models;

public class Reservation : BaseAuditableEntity
{
    public string Theme { get; set; } = null!;
    public ReservationStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RoomId { get; set; }
    public string HostId { get; set; } = null!;
    public virtual Room Room { get; set; } = null!;
    public virtual AppUser Host { get; set; } = null!;
    public virtual ICollection<AppUser> Participants { get; set; }

    public Reservation()
    {
        Participants = new List<AppUser>();
    }
}
