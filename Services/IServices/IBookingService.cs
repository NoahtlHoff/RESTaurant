using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;
using RESTaurang.Models;

namespace RESTaurang.Services.IServices
{
    public interface IBookingService
    {
        Task<List<BookingReadDto>> GetAllBookingsAsync();
        Task<BookingReadDto?> GetBookingByIdAsync(int id);
        Task<BookingCreateDto> CreateBookingAsync(BookingCreateDto bookingCreateDto);
        Task<BookingUpdateDto> UpdateBookingAsync(BookingUpdateDto bookingUpdateDto);
        Task<IActionResult> DeleteBookingAsync(int Id);
    }
}
