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
            return await _ctx.Tables
            .Select(t => new TableReadDto(t.Id, t.Number, t.Capacity))
            .AsNoTracking()
            .ToListAsync();
        }

        public async Task<TableReadDto?> GetTableByIdAsync(int id)
        {
            return await _ctx.Tables
                .AsNoTracking()
                .Where(t => t.Id == id)
                .Select(t => new TableReadDto(t.Id, t.Number, t.Capacity))
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateTableAsync(TableCreateDto dto)
        {
            var entity = new Models.Table 
            { 
                Number = dto.Number, 
                Capacity = dto.Capacity 
            };
            _ctx.Tables.Add(entity);

            await _ctx.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> UpdateTableAsync(int id, TableUpdateDto dto)
        {
            var rowsAffected = await _ctx.Tables
                .Where(t => t.Id == id)
                .ExecuteUpdateAsync(upd => upd
                    .SetProperty(t => t.Number, dto.Number)
                    .SetProperty(t => t.Capacity, dto.Capacity));

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTableAsync(int id)
        {
            var rowsAffected = await _ctx.Tables
                .Where(t => t.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }
    }
}
