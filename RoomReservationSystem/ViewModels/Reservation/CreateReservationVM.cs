using RoomReservationSystem.Enums;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.ViewModels.Reservation;

public class CreateReservationVM
{
    public string Theme { get; set; } = null!;
    public ReservationStatus Status { get; set; } = ReservationStatus.Ongoing;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RoomId { get; set; }
    public ICollection<AppUser> Participants { get; set; }
    public CreateReservationVM()
    {
        Participants = new List<AppUser>();
    }
}
