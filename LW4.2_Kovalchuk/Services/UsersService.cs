using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using LW4._2_Kovalchuk.Enum;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LW4._2_Kovalchuk.Repositories;

namespace LW4._2_Kovalchuk.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository _repository;
        private readonly IValidator<UserItem> _validator;
        private readonly IMapper _mapper;
        public UsersService(IUserRepository repository, IValidator<UserItem> validator,IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper= mapper;
        }
        public UserDTO ToDto(UserItem user)
        {
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserItem> CreateAsync(UserItem user)
        {
            var validationResult = await _validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            await _repository.CreateAsync(user);
            return user;
        }

        public async Task<List<UserItem>> GetAllAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<UserItem> GetByIdAsync(int id)
        {
            var user = await _repository.GetAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");
            return user;
        }
        public async Task<UserItem?> GetByEmailAsync(string email) => await _repository.GetByEmailAsync(email);
        public async Task UpdateAsync(int id, UserItem updatedUser)
        {
            var existing = await _repository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            var validationResult = await _validator.ValidateAsync(updatedUser);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            updatedUser.Id = id;
            await _repository.UpdateAsync(updatedUser);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
        public async Task<int> GetCountByRoleAsync(Roles role)
        {
            if (role == Roles.None)
                throw new ArgumentException("Role cannot be None.", nameof(role));
            var users = await _repository.GetAsync();
            return users.Count(u => (u.Role & role) == role); 
        }
    }
}