using RESTaurang.Dtos;

namespace RESTaurang.Services.IServices
{
    public interface IMenuItemService
    {
        Task<List<MenuItemReadDto>> GetAllAsync();
        Task<MenuItemReadDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(MenuItemCreateDto dto);
        Task<bool> UpdateAsync(int id, MenuItemUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
