using RoomReservationSystem.Models;

namespace RoomReservationSystem.Services.Interfaces;

public interface IRoomService
{
    Task<List<Room>> GetAllRoomsAsync();
}
