using System.Data;
using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Models;

public class BookingService
{
    private readonly AppDbContext _db;
    public BookingService(AppDbContext db) { _db = db; }

    private static DateTime End(DateTime start) => start.AddHours(2);

    public async Task<List<Table>> GetAvailableAsync(DateTime startTime, int guests, CancellationToken ct)
    {
        var end = End(startTime);

        return await _db.Tables
            .Where(t => t.Capacity >= guests)
            .Where(t => !_db.Bookings.Any(b =>
                b.TableId_FK == t.Id &&
                b.StartTime < end &&
                b.StartTime.AddHours(2) > startTime))
            .OrderBy(t => t.Capacity)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<Booking> CreateAsync(int tableId, int guests, DateTime startTime, int customerId, CancellationToken ct)
    {
        if (guests <= 0) throw new ArgumentException("Guests must be > 0");

        var end = End(startTime);

        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);

        var table = await _db.Tables.FirstOrDefaultAsync(t => t.Id == tableId, ct);
        if (table == null) throw new KeyNotFoundException("Table not found");
        if (guests > table.Capacity) throw new ArgumentException("Guests exceed table capacity");

        var customerExists = await _db.Customers.AnyAsync(c => c.Id == customerId, ct);
        if (!customerExists) throw new KeyNotFoundException("Customer not found");

        // Överlapp: existing.Start < new.End OCH existing.End > new.Start
        var overlap = await _db.Bookings.AnyAsync(b =>
            b.TableId_FK == table.Id &&
            b.StartTime < end &&
            b.StartTime.AddHours(2) > startTime, ct);

        if (overlap) throw new InvalidOperationException("Time slot already booked");

        var booking = new Booking
        {
            TableId_FK = table.Id,
            CustomerId_FK = customerId,
            StartTime = startTime,
            Guests = guests
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return booking;
    }
}
