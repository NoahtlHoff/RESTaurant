namespace RESTaurang.Data
{
    using System.Security.Cryptography;
    using System.Text;
    using global::RESTaurang.Models;
    using Microsoft.EntityFrameworkCore;

    namespace RESTaurang.Data
    {
        public static class AppDbInitializer
        {
            public static async Task SeedAsync(IServiceProvider services)
            {
                var ctx = services.GetRequiredService<AppDbContext>();
                await ctx.Database.MigrateAsync();

                // --- Admin ---
                if (!await ctx.Admins.AnyAsync())
                {
                    CreatePasswordHash("Passw0rd!", out byte[] hash, out byte[] salt); // change later
                    ctx.Admins.Add(new Admin
                    {
                        Username = "admin",
                        PasswordHash = hash,
                        PasswordSalt = salt
                    });
                }

                // --- Tables ---
                if (!await ctx.Tables.AnyAsync())
                {
                    ctx.Tables.AddRange(new[]
                    {
                    new Table { Number = "T1", Capacity = 2 },
                    new Table { Number = "T2", Capacity = 2 },
                    new Table { Number = "T3", Capacity = 4 },
                    new Table { Number = "T4", Capacity = 4 },
                    new Table { Number = "T5", Capacity = 6 },
                    new Table { Number = "T6", Capacity = 8 },
                });
                }

                // --- Menu Items ---
                if (!await ctx.MenuItems.AnyAsync())
                {
                    ctx.MenuItems.AddRange(new[]
                    {
                    new MenuItem { Name = "Margherita Pizza", Price = 99m, IsPopular = true,
                        Description = "Tomat, mozzarella, basilika.",
                        ImageUrl = "https://picsum.photos/seed/pizza/600/400" },
                    new MenuItem { Name = "Pasta Carbonara", Price = 129m, IsPopular = true,
                        Description = "Guanciale, ägg, pecorino.",
                        ImageUrl = "https://picsum.photos/seed/pasta/600/400" },
                    new MenuItem { Name = "Grillad Lax", Price = 169m, IsPopular = false,
                        Description = "Serveras med citron och örter.",
                        ImageUrl = "https://picsum.photos/seed/salmon/600/400" },
                    new MenuItem { Name = "Caesarsallad", Price = 115m, IsPopular = false,
                        Description = "Klassisk dressing, krutonger, parmesan.",
                        ImageUrl = "https://picsum.photos/seed/salad/600/400" },
                    new MenuItem { Name = "Tiramisu", Price = 79m, IsPopular = true,
                        Description = "Espresso, mascarpone, kakao.",
                        ImageUrl = "https://picsum.photos/seed/tiramisu/600/400" }
                });
                }

                // --- Optional: demo customer ---
                if (!await ctx.Customers.AnyAsync())
                {
                    ctx.Customers.Add(new Customer { Name = "Testkund", Phone = "070-0000000" });
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
    }

}
