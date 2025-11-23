using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using MongoDB.Driver;

namespace LW4._2_Kovalchuk.Repositories
{
    public class UsersRepository : IUserRepository
    {
        private readonly IMongoCollection<UserItem> _collection;

        public UsersRepository(IMongoCollection<UserItem> collection)
        {
            _collection = collection;
        }

        public async Task CreateAsync(UserItem user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            await _collection.InsertOneAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<List<UserItem>> GetAsync()
        {
            var result = await _collection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<UserItem> GetAsync(int id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<UserItem?> GetByEmailAsync(string email)
        => await _collection.Find(u => u.Email == email).FirstOrDefaultAsync();
        public async Task UpdateAsync(UserItem user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            await _collection.ReplaceOneAsync(x => x.Id == user.Id, user);
        }
    }
}