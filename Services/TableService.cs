using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Dtos;
using RESTaurang.Services.IServices;

namespace RESTaurang.Services
{
    public class TableService : ITableService
    {
        private readonly AppDbContext _ctx;
        public TableService(AppDbContext ctx) { _ctx = ctx; }

        public async Task<List<TableReadDto>> GetAllTablesAsync()
        {
            var dtoTables = await _ctx.Tables
                .Select(t => new TableReadDto(
                    t.Id,
                    t.Number,
                    t.Capacity
                ))
                .ToListAsync();

            return dtoTables;
        }

        public async Task<TableReadDto?> GetTableByIdAsync(int Id)
        {
            var table = await _ctx.Tables
                .FirstOrDefaultAsync(t => t.Id == Id);

            return new TableReadDto(table.Id,table.Number,table.Capacity);
        }

        public async Task<IActionResult> CreateTableAsync(TableCreateDto tableCreateDto)
        {
            await _ctx.Tables.AddAsync(new Models.Table
            {
                Number = tableCreateDto.Number,
                Capacity = tableCreateDto.Capacity
            });
            return await Task.FromResult(new OkResult());
        }

        public async Task<IActionResult> DeleteTableAsync(int Id)
        {
            await _ctx.Tables.Where(T => T.Id == Id).ExecuteDeleteAsync();

            return await Task.FromResult(new OkResult());
        }

        public async Task<IActionResult> UpdateTableAsync(int Id, TableUpdateDto tableUpdateDto)
        {
            await _ctx.Tables
                .Where(t => t.Id == Id)
                .ExecuteUpdateAsync(t => t
                    .SetProperty(t => t.Number, tableUpdateDto.Number)
                    .SetProperty(t => t.Capacity, tableUpdateDto.Capacity)
                );
            return await Task.FromResult(new OkResult());
        }
    }
}
