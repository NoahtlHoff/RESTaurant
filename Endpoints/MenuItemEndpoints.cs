using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Models;

namespace RESTaurang.Endpoints
{
    public static class MenuItemEndpoints
    {
        public static void MapMenuItemEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/menuitems", async (AppDbContext db) =>
                await db.MenuItems.ToListAsync());

            app.MapGet("/api/menuitems/{id}", async (AppDbContext db, int id) =>
                await db.MenuItems.FindAsync(id) is MenuItem m ? Results.Ok(m) : Results.NotFound());

            app.MapPost("/api/menuitems", async (AppDbContext db, MenuItem item) =>
            {
                db.MenuItems.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/api/menuitems/{item.Id}", item);
            });

            app.MapPut("/api/menuitems/{id}", async (AppDbContext db, int id, MenuItem input) =>
            {
                var item = await db.MenuItems.FindAsync(id);
                if (item is null) return Results.NotFound();

                item.Name = input.Name;
                item.Price = input.Price;
                item.Description = input.Description;
                item.IsPopular = input.IsPopular;
                item.ImageUrl = input.ImageUrl;

                await db.SaveChangesAsync();
                return Results.NoContent();
            });

            app.MapDelete("/api/menuitems/{id}", async (AppDbContext db, int id) =>
            {
                var item = await db.MenuItems.FindAsync(id);
                if (item is null) return Results.NotFound();

                db.MenuItems.Remove(item);
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
