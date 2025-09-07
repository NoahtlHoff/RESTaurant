using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTaurang.Dtos;
using RESTaurang.Models;
using RESTaurang.Services;
using RESTaurang.Services.IServices;

namespace RESTaurang.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/menu-items")]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuitem;
        public MenuItemController(IMenuItemService menuitem) => _menuitem = menuitem;

        [HttpGet]
        public async Task<ActionResult<List<MenuItemReadDto>>> GetAll()
        {
            return Ok(await _menuitem.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemReadDto>> GetById(int id)
        {
            var item = await _menuitem.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(MenuItemCreateDto dto)
        {
            var id = await _menuitem.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MenuItemUpdateDto dto)
        {
            var success = await _menuitem.UpdateAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _menuitem.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
