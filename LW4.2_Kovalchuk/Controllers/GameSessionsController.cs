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
    public class GameSessionsController : ControllerBase
    {
        private readonly IGameSessionsService _service;

        public GameSessionsController(IGameSessionsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameSessionItem>>> GetAll()
        {
            var sessions = await _service.GetAllAsync();
            return Ok(sessions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameSessionItem>> GetById(int id)
        {
            try
            {
                var session = await _service.GetByIdAsync(id);
                return Ok(session);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Ігрову сесію з ID={id} не знайдено.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<GameSessionItem>> Create(GameSessionItem newSession)
        {
            try
            {
                var createdSession = await _service.CreateAsync(newSession);
                return CreatedAtAction(nameof(GetById), new { id = createdSession.Id }, createdSession);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GameSessionItem updatedSession)
        {
            try
            {
                await _service.UpdateAsync(id, updatedSession);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                return NotFound($"Сесію з ID={id} не знайдено.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}