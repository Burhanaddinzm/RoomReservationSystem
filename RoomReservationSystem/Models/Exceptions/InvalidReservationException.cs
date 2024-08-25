namespace RoomReservationSystem.Models.Exceptions;

public class InvalidReservationException : Exception
{
    public InvalidReservationException(string? message) : base(message)
    {
    }
}
