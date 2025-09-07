using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RESTaurang.Models;

namespace RESTaurang.Data;

public static class AppDbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var ctx = services.GetRequiredService<AppDbContext>();
        await ctx.Database.MigrateAsync();

        // --- Admins ---
        if (!await ctx.Admins.AnyAsync())
        {
            var usernames = new[] { "admin", "manager", "chef", "waiter", "bartender" };
            foreach (var username in usernames)
            {
                CreatePasswordHash("Passw0rd!", out var hash, out var salt);
                ctx.Admins.Add(new Admin
                {
                    Username = username,
                    PasswordHash = hash,
                    PasswordSalt = salt
                });
            }
        }

        // --- Tables ---
        if (!await ctx.Tables.AnyAsync())
        {
            var tables = new List<Table>();
            for (int i = 1; i <= 40; i++)
            {
                tables.Add(new Table
                {
                    Number = $"T{i:D2}",
                    Capacity = i % 5 == 0 ? 8 : (i % 3 == 0 ? 6 : (i % 2 == 0 ? 4 : 2))
                });
            }
            ctx.Tables.AddRange(tables);
        }

        // --- Menu Items ---
        if (!await ctx.MenuItems.AnyAsync())
        {
            ctx.MenuItems.AddRange(new[]
            {
                // Pizzas
                new MenuItem { Name = "Margherita Pizza", Price = 99m, IsPopular = true,
                    Description = "Tomat, mozzarella, basilika.",
                    ImageUrl = "https://picsum.photos/seed/pizza1/600/400" },
                new MenuItem { Name = "Pepperoni Pizza", Price = 119m, IsPopular = true,
                    Description = "Tomat, mozzarella, pepperoni.",
                    ImageUrl = "https://picsum.photos/seed/pizza2/600/400" },
                new MenuItem { Name = "Veggie Pizza", Price = 109m, IsPopular = false,
                    Description = "Paprika, oliver, lök, zucchini.",
                    ImageUrl = "https://picsum.photos/seed/pizza3/600/400" },

                // Pasta
                new MenuItem { Name = "Pasta Carbonara", Price = 129m, IsPopular = true,
                    Description = "Guanciale, ägg, pecorino.",
                    ImageUrl = "https://picsum.photos/seed/pasta1/600/400" },
                new MenuItem { Name = "Pasta Bolognese", Price = 135m, IsPopular = true,
                    Description = "Nötkött, tomatsås, parmesan.",
                    ImageUrl = "https://picsum.photos/seed/pasta2/600/400" },

                // Mains
                new MenuItem { Name = "Grillad Lax", Price = 169m, IsPopular = false,
                    Description = "Serveras med citron och örter.",
                    ImageUrl = "https://picsum.photos/seed/salmon/600/400" },
                new MenuItem { Name = "Entrecôte", Price = 249m, IsPopular = true,
                    Description = "Stekt till perfektion, med pommes.",
                    ImageUrl = "https://picsum.photos/seed/steak/600/400" },

                // Salads
                new MenuItem { Name = "Caesarsallad", Price = 115m, IsPopular = false,
                    Description = "Klassisk dressing, krutonger, parmesan.",
                    ImageUrl = "https://picsum.photos/seed/salad1/600/400" },
                new MenuItem { Name = "Grekisk Sallad", Price = 99m, IsPopular = false,
                    Description = "Fetaost, oliver, tomat, gurka.",
                    ImageUrl = "https://picsum.photos/seed/salad2/600/400" },

                // Desserts
                new MenuItem { Name = "Tiramisu", Price = 79m, IsPopular = true,
                    Description = "Espresso, mascarpone, kakao.",
                    ImageUrl = "https://picsum.photos/seed/tiramisu/600/400" },
                new MenuItem { Name = "Pannacotta", Price = 69m, IsPopular = false,
                    Description = "Vanilj, bärsås.",
                    ImageUrl = "https://picsum.photos/seed/pannacotta/600/400" },

                // Drinks
                new MenuItem { Name = "Espresso", Price = 35m, IsPopular = true,
                    Description = "En stark shot kaffe.",
                    ImageUrl = "https://picsum.photos/seed/coffee/600/400" },
                new MenuItem { Name = "Rött vin", Price = 89m, IsPopular = true,
                    Description = "Husets röda, glas.",
                    ImageUrl = "https://picsum.photos/seed/wine/600/400" },
                new MenuItem { Name = "Vitt vin", Price = 89m, IsPopular = false,
                    Description = "Husets vita, glas.",
                    ImageUrl = "https://picsum.photos/seed/wine2/600/400" }
            });
        }

        // --- Customers ---
        if (!await ctx.Customers.AnyAsync())
        {
            var customers = new List<Customer>();
            for (int i = 1; i <= 30; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"Kund {i}",
                    Phone = $"070-{i:D7}"
                });
            }
            ctx.Customers.AddRange(customers);
        }

        await ctx.SaveChangesAsync();

        // --- Bookings ---
        if (!await ctx.Bookings.AnyAsync())
        {
            var rnd = new Random();
            var customers = await ctx.Customers.ToListAsync();
            var tables = await ctx.Tables.ToListAsync();
            var bookings = new List<Booking>();

            for (int i = 0; i < 100; i++)
            {
                var startDay = DateTime.Today.AddDays(rnd.Next(0, 7)); // within 7 days
                var startHour = rnd.Next(17, 22); // dinner hours 17–21
                var start = startDay.AddHours(startHour);

                var table = tables[rnd.Next(tables.Count)];
                var customer = customers[rnd.Next(customers.Count)];
                var guests = rnd.Next(1, table.Capacity + 1);

                bookings.Add(new Booking
                {
                    StartTime = start,
                    Guests = guests,
                    TableId = table.Id,
                    CustomerId = customer.Id
                });
            }

            ctx.Bookings.AddRange(bookings);
        }

        await ctx.SaveChangesAsync();
    }

    private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}
