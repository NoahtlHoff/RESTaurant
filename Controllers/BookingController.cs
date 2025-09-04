using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Dtos;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _svc;
    private readonly AppDbContext _db;
    public BookingsController(BookingService svc, AppDbContext db) { _svc = svc; _db = db; }

    // GET /api/bookings/available?startTime=2025-09-03T18:00:00Z&guests=4
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime startTime, [FromQuery] int guests, CancellationToken ct)
    {
        var result = await _svc.GetAvailableAsync(startTime, guests, ct);
        return Ok(result);
    }

    // POST /api/bookings
    [Authorize] // skriv kräver JWT
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto, CancellationToken ct)
    {
        try
        {
            var b = await _svc.CreateAsync(
                tableId: dto.TableId_FK,
                guests: dto.Guests,
                startTime: dto.StartTime,
                customerId: dto.CustomerId_FK,
                ct: ct);

            var read = new BookingReadDto(
                Id: b.Id,
                StartTime: b.StartTime,
                Guests: b.Guests,
                CustomerId: b.CustomerId_FK,
                TableId: b.TableId_FK
            );

            return CreatedAtAction(nameof(GetById), new { id = b.Id }, read);
        }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); } // 409
    }

    // GET /api/bookings/123
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var b = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (b == null) return NotFound();

        var read = new BookingReadDto(
            Id: b.Id,
            StartTime: b.StartTime,
            Guests: b.Guests,
            CustomerId: b.CustomerId_FK,
            TableId: b.TableId_FK
        );

        return Ok(read);
    }
}
