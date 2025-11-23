using LW4._2_Kovalchuk.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LW4._2_Kovalchuk.Interfaces
{
    public interface IGameSessionsService
    {
        Task<GameSessionItem> CreateAsync(GameSessionItem session);
        Task<List<GameSessionItem>> GetAllAsync();
        Task<GameSessionItem> GetByIdAsync(int id);
        Task UpdateAsync(int id, GameSessionItem updatedSession);
        Task DeleteAsync(int id);
    }
}