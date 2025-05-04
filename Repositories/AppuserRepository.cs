using Sandbox_Calc.Model;
using Microsoft.EntityFrameworkCore;

namespace Sandbox_Calc.Repositories
{
    public interface IAppuserRepository
    {
        Task<Appuser> AddUserAsync(Appuser _appuser);
        Task<Appuser> UpdateUserAsync(Appuser _appuser);
        Task<IEnumerable<Appuser>> GetUserAsync();
        Task<Appuser?> FindUserByIdAsync(int id);
        Task DeleteUserAsync(Appuser _appuser);
    }
    public class AppuserRepository(AppDbContext context) : IAppuserRepository
    {
        public async Task<Appuser> AddUserAsync(Appuser _appuser)
        {
            context.APPUSER.Add(_appuser);
            await context.SaveChangesAsync();
            return _appuser;  // returning created product, it will automatically fetch `Id`
        }

        public async Task<Appuser> UpdateUserAsync(Appuser _appuser)
        {
            context.APPUSER.Update(_appuser);
            await context.SaveChangesAsync();
            return _appuser;
        }

        public async Task DeleteUserAsync(Appuser _appuser)
        {
            context.APPUSER.Remove(_appuser);
            await context.SaveChangesAsync();
        }

        public async Task<Appuser?> FindUserByIdAsync(int id)
        {
            var _appuser = await context.APPUSER.FindAsync(id);
            return _appuser;
        }

        public async Task<IEnumerable<Appuser>> GetUserAsync()
        {
            return await context.APPUSER.ToListAsync();
        }


    }
}