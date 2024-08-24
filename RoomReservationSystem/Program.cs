using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Implementations;
using RoomReservationSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
    {
        optionsBuilder.UseSqlServer(
            builder.Configuration.GetConnectionString("Default"),
            sqlOptions => sqlOptions.MigrationsHistoryTable("Migrations"));
    });

    builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.Lockout.AllowedForNewUsers = false;
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(cookieAuthOptions =>
    {
        cookieAuthOptions.LoginPath = "/auth/login";
        cookieAuthOptions.LogoutPath = "/auth/logout";
    });

    builder.Services.AddScoped<IRoomService, RoomService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddTransient<IEmailSender, EmailSender>();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
