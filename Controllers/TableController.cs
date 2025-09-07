using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;
using RESTaurang.Services.IServices;

namespace RESTaurang.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/tables")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tables;
        public TableController(ITableService tables) { _tables = tables; }

        [HttpGet]
        public async Task<ActionResult<List<TableReadDto>>> GetAll()
        {
            var result = await _tables.GetAllTablesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TableReadDto>> GetById(int id)
        {
            var dto = await _tables.GetTableByIdAsync(id);
            if (dto is null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TableCreateDto dto)
        {
            var id = await _tables.CreateTableAsync(dto);
            var created = await _tables.GetTableByIdAsync(id);
            return CreatedAtAction(nameof(GetById), new { id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] TableUpdateDto dto)
        {
            var success = await _tables.UpdateTableAsync(id, dto);
            if (!success) return NotFound();

            var updated = await _tables.GetTableByIdAsync(id);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _tables.DeleteTableAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
