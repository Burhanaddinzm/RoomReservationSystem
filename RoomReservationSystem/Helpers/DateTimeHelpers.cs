namespace RoomReservationSystem.Helpers;

public static class DateTimeHelpers
{
    public static DateTime RoundToNext30Minutes(DateTime dateTime)
    {
        var ms = TimeSpan.FromMinutes(30).TotalMilliseconds;
        var roundedTicks = Math.Ceiling(dateTime.Ticks / ms) * ms;
        return new DateTime((long)roundedTicks, dateTime.Kind);
    }
}
