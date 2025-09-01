using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Models;

namespace RESTaurang.Endpoints
{
    public static class BookingEndpoints
    {
        public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/bookings", async (AppDbContext db) => await db.Bookings.ToListAsync());

            app.MapGet("/api/bookings/{id}", async (AppDbContext db, int id) =>
                await db.Bookings.FindAsync(id) is Booking b ? Results.Ok(b) : Results.NotFound());

            app.MapPost("/api/bookings", async (AppDbContext db, Booking input) =>
            {
                var start = input.StartTime;
                var end = start.AddHours(2);

                var overlaps = await db.Bookings.AnyAsync(b =>
                    b.TableId_FK == input.TableId_FK &&
                    start < b.StartTime.AddHours(2) &&
                    end > b.StartTime);

                if (overlaps) return Results.Conflict(new { message = "Bordet är upptaget i den perioden." });

                db.Bookings.Add(input);
                await db.SaveChangesAsync();
                return Results.Created($"/api/bookings/{input.Id}", input);
            });

            app.MapPut("/api/bookings/{id}", async (AppDbContext db, int id, Booking input) =>
            {
                var b = await db.Bookings.FindAsync(id);
                if (b is null) return Results.NotFound();

                var start = input.StartTime;
                var end = start.AddHours(2);

                var overlaps = await db.Bookings.AnyAsync(x =>
                    x.Id != id &&
                    x.TableId_FK == input.TableId_FK &&
                    start < x.StartTime.AddHours(2) &&
                    end > x.StartTime);

                if (overlaps) return Results.Conflict(new { message = "Bordet är upptaget i den perioden." });

                b.StartTime = input.StartTime;
                b.Guests = input.Guests;
                b.CustomerId_FK = input.CustomerId_FK;
                b.TableId_FK = input.TableId_FK;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/api/bookings/{id}", async (AppDbContext db, int id) =>
            {
                var b = await db.Bookings.FindAsync(id);
                if (b is null) return Results.NotFound();
                db.Bookings.Remove(b);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
