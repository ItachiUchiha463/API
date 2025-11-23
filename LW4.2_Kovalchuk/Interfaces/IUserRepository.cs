using LW4._2_Kovalchuk.Models;
namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IUserRepository
    {
        Task CreateAsync(UserItem user);
        Task<List<UserItem>> GetAsync();
        Task<UserItem> GetAsync(int id);
        Task UpdateAsync(UserItem user);
        Task DeleteAsync(int id);
        Task<UserItem?> GetByEmailAsync(string email);
    }
}
