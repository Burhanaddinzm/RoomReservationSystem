using RoomReservationSystem.Models;
using RoomReservationSystem.Services.Interfaces;
using RoomReservationSystem.ViewModels.Auth;

namespace RoomReservationSystem.Data;

public static class Seed
{
    public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            if (await userService.IsTableEmpty())
            {
                var testUsers = new List<RegisterVM>
                {
                    new RegisterVM
                    {
                        Name = "John",
                        Surname = "Smith",
                        Email = "test@test.com",
                        Password = "Test1234",
                        ConfirmPassword = "Test1234"
                    },
                    new RegisterVM
                    {
                        Name = "Alice",
                        Surname = "Johnson",
                        Email = "alice@test.com",
                        Password = "Password1!",
                        ConfirmPassword = "Password1!"
                    },
                    new RegisterVM
                    {
                         Name = "Bob",
                         Surname = "Williams",
                         Email = "bob@test.com",
                         Password = "Password2@",
                         ConfirmPassword = "Password2@"
                    },
                    new RegisterVM
                    {
                          Name = "Charlie",
                          Surname = "Brown",
                          Email = "charlie@test.com",
                          Password = "Password3#",
                          ConfirmPassword = "Password3#"
                    },
                    new RegisterVM
                    {
                           Name = "David",
                           Surname = "Davis",
                           Email = "david@test.com",
                           Password = "Password4$",
                           ConfirmPassword = "Password4$"
                    },
                    new RegisterVM
                    {
                           Name = "Eve",
                           Surname = "Miller",
                           Email = "eve@test.com",
                           Password = "Password5%",
                           ConfirmPassword = "Password5%"
                    },
                    new RegisterVM
                    {
                           Name = "Frank",
                           Surname = "Wilson",
                           Email = "frank@test.com",
                           Password = "Password6^",
                           ConfirmPassword = "Password6^"
                    },
                    new RegisterVM
                    {
                           Name = "Grace",
                           Surname = "Moore",
                           Email = "grace@test.com",
                           Password = "Password7&",
                           ConfirmPassword = "Password7&"
                    },
                    new RegisterVM
                    {
                           Name = "Henry",
                           Surname = "Taylor",
                           Email = "henry@test.com",
                           Password = "Password8*",
                           ConfirmPassword = "Password8*"
                    },
                    new RegisterVM
                    {
                          Name = "Ivy",
                          Surname = "Anderson",
                          Email = "ivy@test.com",
                          Password = "Password9(",
                          ConfirmPassword = "Password9("
                    },
                    new RegisterVM
                    {
                           Name = "Jack",
                           Surname = "Thomas",
                           Email = "jack@test.com",
                           Password = "Password10)",
                           ConfirmPassword = "Password10)"
                    },
                    new RegisterVM
                    {
                            Name = "Kate",
                            Surname = "Jackson",
                            Email = "kate@test.com",
                            Password = "Password11_",
                            ConfirmPassword = "Password11_"
                    },
                    new RegisterVM
                    {
                           Name = "Leo",
                           Surname = "White",
                           Email = "leo@test.com",
                           Password = "Password12+",
                           ConfirmPassword = "Password12+"
                    },
                    new RegisterVM
                    {
                           Name = "Mia",
                           Surname = "Harris",
                           Email = "mia@test.com",
                           Password = "Password13-",
                           ConfirmPassword = "Password13-"
                    },
                    new RegisterVM
                    {
                           Name = "Sam",
                           Surname = "Clark",
                           Email = "sam@test.com",
                           Password = "Password19#",
                           ConfirmPassword = "Password19#"
                    }
                };

                foreach (var user in testUsers)
                {
                    await userService.CreateAsync(user);
                }
            }
        }
    }

    public static async Task SeedRoomsAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roomService = scope.ServiceProvider.GetRequiredService<IRoomService>();
            if (await roomService.IsTableEmpty())
            {
                for (int i = 0; i < 12; i++)
                {
                    await roomService.CreateRoomAsync(new Room { Name = $"Room {i + 1}", IsAvailable = true });
                }
            }
        }
    }
}
