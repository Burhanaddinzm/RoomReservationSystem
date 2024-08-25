using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Enums;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;
using RoomReservationSystem.ViewModels.Reservation;

namespace RoomReservationSystem.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations.Include(x => x.Room)
                                          .Include(x => x.Host)
                                          .Include(x => x.Participants)
                                          .ToListAsync();
    }

    public async Task CreateAsync(CreateReservationVM reservationVM, AppUser host)
    {
        var participants = await _context.Users
                                  .Where(x => reservationVM.ParticipantIds.Contains(x.Id))
                                  .ToListAsync();

        var reservation = new Reservation
        {
            Theme = reservationVM.Theme,
            Status = reservationVM.Status,
            StartDate = reservationVM.StartDate,
            EndDate = reservationVM.EndDate,
            RoomId = reservationVM.RoomId,
            HostId = host.Id,
            Participants = participants
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task CancelReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        reservation.Status = ReservationStatus.Canceled;
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }
}
