using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model; // Pastikan ada model APPUSER di folder Model

namespace Sandbox_Calc.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: api/category
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategories([FromHeader] string username)
        {
            
            var data = await _appDbContext.Category
                .Where(t => t.user_created == username)
                .ToListAsync();

            return Ok(data);
        }

        // GET: api/category/1
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetCategory(int id)
        {
            var user = await _appDbContext.Category.FindAsync(id);
            if (user == null)
                return NotFound($"Category with id: {id} was not found.");
           
            return Ok(user);
        }

        // POST: api/category
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Category category)
        {
            if (category == null)
                return BadRequest("data is required.");

             
            _appDbContext.Category.Add(category);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.id }, category);
        }


        // PUT: api/appuser/1
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.id)
                return BadRequest("ID mismatch");

            var existing = await _appDbContext.Category.FindAsync(id);
            if (existing == null)
                return NotFound($"User with id: {id} was not found.");

            // Update fields here
            existing.category = category.category;
            

            _appDbContext.Category.Update(existing);
            await _appDbContext.SaveChangesAsync();

            return Ok(existing);
        }

        // DELETE: api/category/1
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _appDbContext.Category.FindAsync(id);
            if (user == null)
                return NotFound($"Data with id: {id} was not found.");

            _appDbContext.Category.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
