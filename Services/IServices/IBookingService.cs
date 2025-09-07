using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;
using RESTaurang.Models;

namespace RESTaurang.Services.IServices
{
    public interface IBookingService
    {
        Task<List<BookingReadDto>> GetAllAsync();
        Task<BookingReadDto?> GetByIdAsync(int id);
        Task<int?> CreateAsync(BookingCreateDto dto);
        Task<BookingReadDto?> UpdateAsync(int id, BookingUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<List<TableAvailabilityDto>> GetAvailableTablesAsync(DateTime? startTime, int? guests);
    }
}
