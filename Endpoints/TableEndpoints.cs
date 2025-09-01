using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Models;

namespace RESTaurang.Endpoints
{
    public static class TableEndpoints
    {
        public static void MapTableEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/tables", async (AppDbContext db) => await db.Tables.ToListAsync());

            app.MapGet("/api/tables/{id}", async (AppDbContext db, int id) =>
                await db.Tables.FindAsync(id) is Table t ? Results.Ok(t) : Results.NotFound());

            app.MapPost("/api/tables", async (AppDbContext db, Table input) =>
            {
                db.Tables.Add(input);
                await db.SaveChangesAsync();
                return Results.Created($"/api/tables/{input.Id}", input);
            });

            app.MapPut("/api/tables/{id}", async (AppDbContext db, int id, Table input) =>
            {
                var t = await db.Tables.FindAsync(id);
                if (t is null) return Results.NotFound();
                t.Number = input.Number;
                t.Capacity = input.Capacity;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/api/tables/{id}", async (AppDbContext db, int id) =>
            {
                var t = await db.Tables.FindAsync(id);
                if (t is null) return Results.NotFound();
                db.Tables.Remove(t);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
