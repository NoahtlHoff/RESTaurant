using RESTaurang.Dtos;

namespace RESTaurang.Services.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerReadDto>> GetAllAsync();
        Task<CustomerReadDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CustomerCreateDto dto);
        Task<CustomerReadDto?> UpdateAsync(int id, CustomerUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
