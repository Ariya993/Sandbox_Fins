using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Sandbox_Calc.Data;
using Sandbox_Calc.Model; // Pastikan ada model APPUSER di folder Model

namespace Sandbox_Calc.Controllers
{
    [Route("api/appuser")]
    [ApiController]
    public class AppuserController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public AppuserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: api/appuser
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _appDbContext.APPUSER.ToListAsync();
            foreach (var user in users)
            {
                user.Password = null;
            }

            return Ok(users);
        }

        // GET: api/appuser/1
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _appDbContext.APPUSER.FindAsync(id);
            if (user == null)
                return NotFound($"User with id: {id} was not found.");
            user.Password = null;

            return Ok(user);
        }

        // POST: api/appuser
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] Appuser newUser)
        {
            if (newUser == null)
                return BadRequest("User data is required.");

            bool usernameExists = await _appDbContext.APPUSER
                .AnyAsync(u => u.Username == newUser.Username);

            if (usernameExists)
                return Conflict($"Username '{newUser.Username}' is already taken.");

            // 🔒 Hash password sebelum disimpan
            newUser.Password = HashHelpers.ToMd5(newUser.Password!);

            _appDbContext.APPUSER.Add(newUser);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }


        // PUT: api/appuser/1
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Appuser updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID mismatch");

            var existingUser = await _appDbContext.APPUSER.FindAsync(id);
            if (existingUser == null)
                return NotFound($"User with id: {id} was not found.");

            // Update fields here
            existingUser.Username = updatedUser.Username;
            // Jika password diisi, hash dan update
            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                existingUser.Password = HashHelpers.ToMd5(updatedUser.Password);
            }
            existingUser.Nama = updatedUser.Nama;
            // Tambah properti lain sesuai model

            _appDbContext.APPUSER.Update(existingUser);
            await _appDbContext.SaveChangesAsync();

            return Ok(existingUser);
        }

        // DELETE: api/appuser/1
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _appDbContext.APPUSER.FindAsync(id);
            if (user == null)
                return NotFound($"User with id: {id} was not found.");

            _appDbContext.APPUSER.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
