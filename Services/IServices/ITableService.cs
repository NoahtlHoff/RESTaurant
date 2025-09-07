using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;

namespace RESTaurang.Services.IServices
{
    public interface ITableService
    {
        Task<List<TableReadDto>> GetAllTablesAsync();
        Task<TableReadDto?> GetTableByIdAsync(int id);
        Task<int> CreateTableAsync(TableCreateDto dto);
        Task<bool> UpdateTableAsync(int id, TableUpdateDto dto);
        Task<bool> DeleteTableAsync(int id);
    }
}
