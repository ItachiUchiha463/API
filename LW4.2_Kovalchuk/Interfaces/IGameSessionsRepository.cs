using LW4._2_Kovalchuk.Models;

namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IGameSessionsRepository
    {
        Task CreateAsync(GameSessionItem user);
        Task<List<GameSessionItem>> GetAsync();
        Task<GameSessionItem> GetAsync(int id);
        Task UpdateAsync(GameSessionItem user);
        Task DeleteAsync(int id);
    }
}
