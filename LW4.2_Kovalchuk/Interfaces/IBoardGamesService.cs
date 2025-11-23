using LW4._2_Kovalchuk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IBoardGamesService
    {
        Task<BoardGameItem> CreateAsync(BoardGameItem game);
        Task<List<BoardGameItem>> GetAllAsync();
        Task<BoardGameItem> GetByIdAsync(int id);
        Task UpdateAsync(int id, BoardGameItem updatedGame);
        Task DeleteAsync(int id);
    }
}