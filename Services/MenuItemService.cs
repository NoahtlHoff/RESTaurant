using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Dtos;
using RESTaurang.Services.IServices;

namespace RESTaurang.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly AppDbContext _ctx;
        public MenuItemService(AppDbContext ctx) { _ctx = ctx; }

        public async Task<List<MenuItemReadDto>> GetAllAsync()
        {
            return await _ctx.MenuItems
                .AsNoTracking()
                .Select(m => new MenuItemReadDto(m.Id, m.Name, m.Price, m.Description, m.IsPopular, m.ImageUrl))
                .ToListAsync();
        }

        public async Task<MenuItemReadDto?> GetByIdAsync(int id)
        {
            return await _ctx.MenuItems
                .AsNoTracking()
                .Where(m => m.Id == id)
                .Select(m => new MenuItemReadDto(m.Id, m.Name, m.Price, m.Description, m.IsPopular, m.ImageUrl))
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(MenuItemCreateDto dto)
        {
            var entity = new Models.MenuItem
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                IsPopular = dto.IsPopular,
                ImageUrl = dto.ImageUrl
            };

            _ctx.MenuItems.Add(entity);
            await _ctx.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> UpdateAsync(int id, MenuItemUpdateDto dto)
        {
            var rowsAffected = await _ctx.MenuItems
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(upd => upd
                    .SetProperty(m => m.Name, dto.Name)
                    .SetProperty(m => m.Price, dto.Price)
                    .SetProperty(m => m.Description, dto.Description)
                    .SetProperty(m => m.IsPopular, dto.IsPopular)
                    .SetProperty(m => m.ImageUrl, dto.ImageUrl));

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var rowsAffected = await _ctx.MenuItems
                .Where(m => m.Id == id)
                .ExecuteDeleteAsync();

            return rowsAffected > 0;
        }
    }
}
