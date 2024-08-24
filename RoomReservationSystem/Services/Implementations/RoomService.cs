using Microsoft.AspNetCore.Identity;
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
                                   .OrderBy(x => x.CreatedAt)
                                   .ToListAsync();
    }

    public async Task<Room?> GetRoomAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task CreateRoomAsync(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsTableEmpty()
    {
        return !await _context.Rooms.AnyAsync();
    }
}
