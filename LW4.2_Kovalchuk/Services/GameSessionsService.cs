using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace LW4._2_Kovalchuk.Services
{
    public class GameSessionsService : IGameSessionsService
    {
        private readonly IGameSessionsRepository _sessionsRepository;
        private readonly IValidator<GameSessionItem> _validator;

        private readonly IMapper _mapper;

        public GameSessionsService(IGameSessionsRepository sessionsRepository, IValidator<GameSessionItem> validator,IMapper mapper)
        {
            _sessionsRepository = sessionsRepository;
            _validator = validator;
            _mapper = mapper;
        }
        public GameSessionDTO ToDto(GameSessionItem user)
        {
            return _mapper.Map<GameSessionDTO>(user);
        }
        public async Task<GameSessionItem> CreateAsync(GameSessionItem session)
        {
            var validationResult = await _validator.ValidateAsync(session);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            session.InProgress = true;

            await _sessionsRepository.CreateAsync(session);
            return session;
        }

        public async Task<List<GameSessionItem>> GetAllAsync()
        {
            return await _sessionsRepository.GetAsync();
        }

        public async Task<GameSessionItem> GetByIdAsync(int id)
        {
            var session = await _sessionsRepository.GetAsync(id);
            if (session == null)
                throw new KeyNotFoundException($"Game session with ID {id} not found.");
            return session;
        }

        public async Task UpdateAsync(int id, GameSessionItem updatedSession)
        {
            var existing = await _sessionsRepository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Game session with ID {id} not found.");

            var validationResult = await _validator.ValidateAsync(updatedSession);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            updatedSession.Id = id;
            await _sessionsRepository.UpdateAsync(updatedSession);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _sessionsRepository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Game session with ID {id} not found.");

            await _sessionsRepository.DeleteAsync(id);
        }
    }
}