using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Models;
using RoomReservationSystem.Models.Common;

namespace RoomReservationSystem.Data;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _accessor;
    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor accessor)
    : base(options)
    {
        _accessor = accessor;
    }

    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseAuditableEntity>();
        var currentUserName = _accessor.HttpContext?.User.Identity?.Name ?? "Anonymous";
        var currentIpAddress = _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserName;
                    entry.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                    entry.Entity.IpAddress = currentIpAddress;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedBy = currentUserName;
                    entry.Entity.ModifiedAt = DateTime.UtcNow.AddHours(4);
                    entry.Entity.IpAddress = currentIpAddress;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Reservation>().HasQueryFilter(x => !x.IsDeleted);
        base.OnModelCreating(modelBuilder);
    }
}
