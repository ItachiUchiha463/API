using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace LW4._2_Kovalchuk.Services
{
    public class BoardGamesService : IBoardGamesService
    {
        private readonly IBoardGamesRepository _repository;
        private readonly IValidator<BoardGameItem> _validator;


        private readonly IMapper _mapper;

        public BoardGamesService(IBoardGamesRepository repository, IValidator<BoardGameItem> validator, IMapper mapper)
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }
        public BoardGameDTO ToDto(BoardGameItem user)
        {
            return _mapper.Map<BoardGameDTO>(user);
        }
        public async Task<BoardGameItem> CreateAsync(BoardGameItem game)
        {
            var validationResult = await _validator.ValidateAsync(game);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
            var allGames = await _repository.GetAsync();
            int maxId = allGames.Any() ? allGames.Max(g => g.Id ?? 0) : 0;
            game.Id = maxId + 1;

            await _repository.CreateAsync(game);
            return game;
        }

        public async Task<List<BoardGameItem>> GetAllAsync()
        {
            return await _repository.GetAsync();
        }

        public async Task<BoardGameItem> GetByIdAsync(int id)
        {
            var game = await _repository.GetAsync(id);
            if (game == null)
                throw new KeyNotFoundException($"Board game with ID {id} not found.");
            return game;
        }

        public async Task UpdateAsync(int id, BoardGameItem updatedGame)
        {
            var existing = await _repository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Board game with ID {id} not found.");

            var validationResult = await _validator.ValidateAsync(updatedGame);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            updatedGame.Id = id;
            await _repository.UpdateAsync(updatedGame);
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repository.GetAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Board game with ID {id} not found.");

            await _repository.DeleteAsync(id);
        }
    }
}