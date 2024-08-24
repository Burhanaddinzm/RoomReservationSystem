using RoomReservationSystem.Data;
using RoomReservationSystem.Services.Interfaces;

namespace RoomReservationSystem.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }
}
