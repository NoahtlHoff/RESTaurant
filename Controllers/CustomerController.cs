using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTaurang.Data;
using RESTaurang.Dtos;
using RESTaurang.Services.IServices;

namespace RESTaurang.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customers;
        public CustomersController(ICustomerService customers) => _customers = customers;

        [HttpGet]
        public async Task<ActionResult<List<CustomerReadDto>>> GetAll()
            => Ok(await _customers.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerReadDto>> GetById(int id)
        {
            var dto = await _customers.GetByIdAsync(id);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto dto)
        {
            var id = await _customers.CreateAsync(dto);
            var created = await _customers.GetByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomerReadDto>> Update(int id, [FromBody] CustomerUpdateDto dto)
        {
            var updated = await _customers.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
            => await _customers.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
