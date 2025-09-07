using Microsoft.EntityFrameworkCore;
using RESTaurang.Data;
using RESTaurang.Dtos;
using RESTaurang.Services.IServices;

namespace RESTaurang.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _ctx;
        public CustomerService(AppDbContext ctx) => _ctx = ctx;

        public async Task<List<CustomerReadDto>> GetAllAsync()
        {
            return await _ctx.Customers
                .AsNoTracking()
                .Select(c => new CustomerReadDto(c.Id, c.Name, c.Phone))
                .ToListAsync();
        }

        public async Task<CustomerReadDto?> GetByIdAsync(int id)
        {
            return await _ctx.Customers
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Select(c => new CustomerReadDto(c.Id, c.Name, c.Phone))
                .FirstOrDefaultAsync();
        }
            

        public async Task<int> CreateAsync(CustomerCreateDto dto)
        {
            var entity = new Models.Customer { Name = dto.Name, Phone = dto.Phone };
            _ctx.Customers.Add(entity);
            await _ctx.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CustomerReadDto?> UpdateAsync(int id, CustomerUpdateDto dto)
        {
            var entity = await _ctx.Customers.FindAsync(id);
            if (entity is null) return null;

            entity.Name = dto.Name;
            entity.Phone = dto.Phone;
            await _ctx.SaveChangesAsync();

            return new CustomerReadDto(entity.Id, entity.Name, entity.Phone);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _ctx.Customers.FindAsync(id);
            if (entity is null) return false;

            _ctx.Customers.Remove(entity);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
