using RoomReservationSystem.Models.Common;

namespace RoomReservationSystem.Models;

public class Room : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public int Size { get; set; } = 12;
    public bool IsAvailable { get; set; } = false;
    public virtual ICollection<Reservation> Reservations { get; set; }
    public Room()
    {
        Reservations = new List<Reservation>();
    }
}
