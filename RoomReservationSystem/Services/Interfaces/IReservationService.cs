
using RoomReservationSystem.Models;
using RoomReservationSystem.ViewModels.Reservation;

namespace RoomReservationSystem.Services.Interfaces;

public interface IReservationService
{
    Task CancelReservationAsync(int id);
    Task<List<Reservation>> GetAllReservationsAsync();
    Task<Reservation?> GetReservationAsync(int id);
    Task CreateAsync(CreateReservationVM reservationVM, AppUser host, Room room);
    Task UpdateStatus();
}
