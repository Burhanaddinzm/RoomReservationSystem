using RoomReservationSystem.Models;

namespace RoomReservationSystem.Services.Interfaces;

public interface IRoomService
{
    Task<List<Room>> GetAllRoomsAsync();
    Task CreateRoomAsync(Room room);
    Task<bool> IsTableEmpty();
    Task<Room?> GetRoomAsync(int id);
}
