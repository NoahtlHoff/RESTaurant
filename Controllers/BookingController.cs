using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;
using RESTaurang.Services;
using RESTaurang.Services.IServices;

namespace RESTaurang.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookings;
    public BookingsController(IBookingService bookings) => _bookings = bookings;

    [HttpGet]
    public async Task<ActionResult<List<BookingReadDto>>> GetAll()
        => Ok(await _bookings.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BookingReadDto>> GetById(int id)
    {
        var dto = await _bookings.GetByIdAsync(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
    {
        var id = await _bookings.CreateAsync(dto);
        if (id is null)
            return Conflict(new { message = "Error: Table not available at this time" });

        var created = await _bookings.GetByIdAsync(id.Value);
        return CreatedAtAction(nameof(GetById), new { id = id.Value }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BookingReadDto>> Update(int id, [FromBody] BookingUpdateDto dto)
    {
        var exists = await _bookings.GetByIdAsync(id);
        if (exists is null) return NotFound();

        var updated = await _bookings.UpdateAsync(id, dto);
        if (updated is null)
            return Conflict(new { message = "Time slot unavailable or capacity too low." });

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
        => await _bookings.DeleteAsync(id) ? NoContent() : NotFound();

    [HttpGet("available")]
    public async Task<ActionResult<List<TableAvailabilityDto>>> GetAvailability([FromQuery] DateTime? start, [FromQuery] int? guests)
    {
        var tables = await _bookings.GetAvailableTablesAsync(start, guests);
        return Ok(tables);
    }
}
