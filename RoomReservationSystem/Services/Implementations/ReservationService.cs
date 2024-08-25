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
    private readonly IRoomService _roomService;
    private readonly IEmailSender _emailSender;
    public ReservationService(
        AppDbContext context,
        IRoomService roomService,
        IEmailSender emailSender)
    {
        _context = context;
        _roomService = roomService;
        _emailSender = emailSender;
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations.Include(x => x.Room)
                                          .Include(x => x.Host)
                                          .Include(x => x.Participants)
                                          .OrderByDescending(x => x.Status == ReservationStatus.Ongoing)
                                          .ThenByDescending(x => x.CreatedAt)
                                          .ToListAsync();
    }

    public async Task<Reservation?> GetReservationAsync(int id)
    {
        return await _context.Reservations.FindAsync(id);
    }

    public async Task CreateAsync(CreateReservationVM reservationVM, AppUser host, Room room)
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

        foreach (var participant in participants)
        {
            string message = $"Hello {participant.FullName}, you have been invited to ${room.Name} by ${host.FullName} at ${reservationVM.StartDate}.";
            await _emailSender.SendEmailAsync(participant.Email!, "Room Invite", message);
        }

        await _roomService.UpdateRoomAvailabilityAsync(reservationVM.RoomId, false);
        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task CancelReservationAsync(int id)
    {
        var reservation = await GetReservationAsync(id);
        reservation!.Status = ReservationStatus.Canceled;
        await _roomService.UpdateRoomAvailabilityAsync(reservation.RoomId, true);
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatus()
    {
        var now = DateTime.UtcNow.AddHours(4);

        var completedReservations = await _context.Reservations
            .Include(x => x.Room)
            .Where(x => x.EndDate <= now && x.Status == ReservationStatus.Ongoing)
            .ToListAsync();

        foreach (var reservation in completedReservations)
        {
            reservation.Status = ReservationStatus.Completed;
            reservation.Room.IsAvailable = true;
            _context.Update(reservation);
        }

        await _context.SaveChangesAsync();
    }
}
