using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;

namespace RESTaurang.Services.IServices
{
    public interface ITableService
    {
        Task<List<TableReadDto>> GetAllTablesAsync();
        Task<TableReadDto?> GetTableByIdAsync(int Id);
        Task<IActionResult> CreateTableAsync(TableCreateDto tableCreateDto);
        Task<IActionResult> UpdateTableAsync(int Id, TableUpdateDto tableUpdateDto);
        Task<IActionResult> DeleteTableAsync(int Id);
    }
}
