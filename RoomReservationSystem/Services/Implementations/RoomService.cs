using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;

namespace RoomReservationSystem.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _context.Rooms.AsNoTracking()
                                   .OrderByDescending(x => x.Id)
                                   .ToListAsync();
    }
}
