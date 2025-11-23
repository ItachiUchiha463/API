using LW4._2_Kovalchuk.Models;

namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IBoardGamesRepository
    {
        Task CreateAsync(BoardGameItem user);
        Task<List<BoardGameItem>> GetAsync();
        Task<BoardGameItem> GetAsync(int id);
        Task UpdateAsync(BoardGameItem user);
        Task DeleteAsync(int id);
    }
}
