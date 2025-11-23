using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using MongoDB.Driver;
using SortTest.Test;

namespace LW4._2_Kovalchuk.Services
{
    public class BoardGamesRepository : IBoardGamesRepository
    {
        private IMongoCollection<BoardGameItem> _collection;

        public BoardGamesRepository(IMongoCollection<BoardGameItem> collection)
        {
            _collection = collection;
        }

        public async Task CreateAsync(BoardGameItem user) => await _collection.InsertOneAsync(user);
        public async Task DeleteAsync(int id) => await _collection.DeleteOneAsync(x => x.Id == id);
        public async Task<List<BoardGameItem>> GetAsync() => await _collection.Find(x => true).ToListAsync();
        public async Task<BoardGameItem> GetAsync(int id) => await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task UpdateAsync(BoardGameItem user) => await _collection.ReplaceOneAsync(x => x.Id == user.Id, user);
    }
}
