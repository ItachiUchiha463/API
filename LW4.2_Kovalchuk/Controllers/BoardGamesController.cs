using Microsoft.AspNetCore.Mvc;
using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using System.Threading.Tasks;
using System;
using FluentValidation;

namespace LW4._2_Kovalchuk.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly IBoardGamesService _service;

        public BoardGamesController(IBoardGamesService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardGameItem>>> GetAll()
        {
            var games = await _service.GetAllAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardGameItem>> GetById(int id)
        {
            try
            {
                var game = await _service.GetByIdAsync(id);
                return Ok(game);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Гру з ID={id} не знайдено.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<BoardGameItem>> Create(BoardGameItem newGame)
        {
            try
            {
                var createdGame = await _service.CreateAsync(newGame);
                return CreatedAtAction(nameof(GetById), new { id = createdGame.Id }, createdGame);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BoardGameItem updatedGame)
        {
            try
            {
                await _service.UpdateAsync(id, updatedGame);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Гру з ID={id} не знайдено.");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Гру з ID={id} не знайдено.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}