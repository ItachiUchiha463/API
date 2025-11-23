using LW4._2_Kovalchuk.Enum;
using LW4._2_Kovalchuk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IUsersService
    {
        Task<UserItem> CreateAsync(UserItem user);
        Task<List<UserItem>> GetAllAsync();
        Task<UserItem> GetByIdAsync(int id);
        Task UpdateAsync(int id, UserItem updatedUser);
        Task<UserItem?> GetByEmailAsync(string email);
        Task DeleteAsync(int id);
        Task<int> GetCountByRoleAsync(Roles role);
    }
}