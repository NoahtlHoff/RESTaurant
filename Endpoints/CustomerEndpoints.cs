using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Models;

namespace RESTaurang.Endpoints
{
    public static class CustomerEndpoints
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/customers", async (AppDbContext db) => await db.Customers.ToListAsync());

            app.MapGet("/api/customers/{id}", async (AppDbContext db, int id) =>
                await db.Customers.FindAsync(id) is Customer c ? Results.Ok(c) : Results.NotFound());

            app.MapPost("/api/customers", async (AppDbContext db, Customer input) =>
            {
                db.Customers.Add(input);
                await db.SaveChangesAsync();
                return Results.Created($"/api/customers/{input.Id}", input);
            });

            app.MapPut("/api/customers/{id}", async (AppDbContext db, int id, Customer input) =>
            {
                var c = await db.Customers.FindAsync(id);
                if (c is null) return Results.NotFound();
                c.Name = input.Name;
                c.Phone = input.Phone;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/api/customers/{id}", async (AppDbContext db, int id) =>
            {
                var c = await db.Customers.FindAsync(id);
                if (c is null) return Results.NotFound();
                db.Customers.Remove(c);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
