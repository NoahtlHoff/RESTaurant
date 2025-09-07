using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Dtos;
using RESTaurang.Models;
using RESTaurang.Services.IServices;

namespace RESTaurang.Services;

public class BookingService : IBookingService
{
    private readonly AppDbContext _ctx;
    public BookingService(AppDbContext ctx) { _ctx = ctx; }

    public async Task<List<BookingReadDto>> GetAllAsync()
    {
        return await _ctx.Bookings.AsNoTracking()
            .Select(b => new BookingReadDto(b.Id, b.StartTime, b.Guests, b.CustomerId, b.TableId))
            .ToListAsync();
    }
        
    public async Task<BookingReadDto?> GetByIdAsync(int id)
    {
        return await _ctx.Bookings.AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookingReadDto(b.Id, b.StartTime, b.Guests, b.CustomerId, b.TableId))
            .FirstOrDefaultAsync();
    }

    public async Task<int?> CreateAsync(BookingCreateDto dto)
    {
        var tableOk = await _ctx.Tables
            .AsNoTracking()
            .AnyAsync(t => 
            t.Id == dto.TableId && 
            t.Capacity >= dto.Guests);
        if (!tableOk) return null;

        var customerOk = await _ctx.Customers
            .AsNoTracking()
            .AnyAsync(c => 
            c.Id == dto.CustomerId);
        if (!customerOk) return null;

        var start = dto.StartTime;
        var end = dto.StartTime.AddHours(2);

        var overlaps = await _ctx.Bookings
            .AsNoTracking()
            .AnyAsync(b =>
            b.TableId == dto.TableId && 
            start < b.StartTime.AddHours(2) &&
            b.StartTime < end);
        if (overlaps) return null;

        var booking = new Models.Booking
        {
            StartTime = dto.StartTime,
            Guests = dto.Guests,
            CustomerId = dto.CustomerId,
            TableId = dto.TableId
        };

        _ctx.Bookings.Add(booking);

        await _ctx.SaveChangesAsync();
        return booking.Id;
    }

    public async Task<BookingReadDto?> UpdateAsync(int id, BookingUpdateDto dto)
    {
        var booking = await _ctx.Bookings.FindAsync(id);
        if (booking is null) return null;

        var tableOk = await _ctx.Tables.AsNoTracking()
            .AnyAsync(t => t.Id == dto.TableId && t.Capacity >= dto.Guests);
        if (!tableOk) return null;

        var start = dto.StartTime;
        var end = dto.StartTime.AddHours(2);
        var overlaps = await _ctx.Bookings.AsNoTracking().AnyAsync(b =>
            b.Id != id &&
            b.TableId == dto.TableId &&
            start < b.StartTime.AddHours(2) &&
            b.StartTime < end);
        if (overlaps) return null;

        booking.StartTime = dto.StartTime;
        booking.Guests = dto.Guests;
        booking.CustomerId = dto.CustomerId;
        booking.TableId = dto.TableId;
        await _ctx.SaveChangesAsync();

        return new BookingReadDto(booking.Id, booking.StartTime, booking.Guests, booking.CustomerId, booking.TableId);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var booking = await _ctx.Bookings.FindAsync(id);
        if (booking is null) return false;

        _ctx.Bookings.Remove(booking);
        await _ctx.SaveChangesAsync();
        return true;
    }

    public async Task<List<TableAvailabilityDto>> GetAvailableTablesAsync(DateTime? startTime, int? guests)
    {
        var date = startTime?.Date ?? DateTime.Today;
        var dayStart = date;
        var dayEnd = date.AddDays(1);

        var timeBlock = new List<(DateTime Start, DateTime End)>();
        for (var t = dayStart.AddHours(10); t < dayEnd; t = t.AddHours(2))
        {
            timeBlock.Add((t, t.AddHours(2)));
        }

        var result = new List<TableAvailabilityDto>();

        var tablesQuery = _ctx.Tables.AsNoTracking();
        if (guests.HasValue)
        {
            tablesQuery = tablesQuery.Where(t => t.Capacity >= guests.Value);
        }

        var allTables = await tablesQuery.ToListAsync();

        foreach (var (start, end) in timeBlock)
        {
            var availableTables = allTables
                .Where(t => !_ctx.Bookings.Any(b =>
                    b.TableId == t.Id &&
                    start < b.StartTime.AddHours(2) &&
                    b.StartTime < end))
                .Select(t => new TableReadDto(t.Id, t.Number, t.Capacity))
                .ToList();

            result.Add(new TableAvailabilityDto(start, end, availableTables));
        }

        return result;
    }
}

