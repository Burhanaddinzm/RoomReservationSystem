using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Enums;
using RoomReservationSystem.Helpers;
using RoomReservationSystem.Models;
using RoomReservationSystem.Models.Exceptions;
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

        var (startDate, endDate) = HandleReservation(reservationVM.StartDate,
                                                     reservationVM.EndDate,
                                                     room.Size,
                                                     participants.Count);

        var reservation = new Reservation
        {
            Theme = reservationVM.Theme,
            Status = reservationVM.Status,
            StartDate = startDate,
            EndDate = endDate,
            RoomId = reservationVM.RoomId,
            HostId = host.Id,
            Participants = participants
        };

        //foreach (var participant in participants)
        //{
        //    string message = $"Hello {participant.FullName}, you have been invited to {room.Name} by {host.FullName} at {reservationVM.StartDate}.";
        //    await _emailSender.SendEmailAsync(participant.Email!, "Room Invite", message);
        //}

        await _roomService.UpdateRoomAvailabilityAsync(reservationVM.RoomId, false);
        host.HostedReservations.Add(reservation);
        _context.Users.Update(host);
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

    private (DateTime, DateTime) HandleReservation(DateTime startDate, DateTime endDate, int roomSize, int participantCount)
    {
        var now = DateTime.UtcNow.AddHours(4);
        var roundedNow = DateTimeHelpers.RoundToNext30Minutes(now);

        if (startDate < roundedNow)
        {
            throw new InvalidReservationException("Start date must be in the future and rounded to the nearest 30 minutes.");
        }
        var roundedStart = DateTimeHelpers.RoundToNext30Minutes(startDate);

        if (endDate <= roundedStart)
        {
            throw new InvalidReservationException("End date must be at least 30 minutes after the start date.");
        }
        var roundedEnd = DateTimeHelpers.RoundToNext30Minutes(endDate);

        if (roomSize <= 0 || roomSize < participantCount)
        {
            throw new InvalidReservationException("Participant count surpasses room size.");
        }
        return (roundedStart, roundedEnd);
    }
}