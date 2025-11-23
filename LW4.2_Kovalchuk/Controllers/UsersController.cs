using Microsoft.AspNetCore.Mvc;
using LW4._2_Kovalchuk.Interfaces;
using LW4._2_Kovalchuk.Models;
using System.Threading.Tasks;
using System;
using FluentValidation;
using LW4._2_Kovalchuk.Enum;
using LW4._2_Kovalchuk.Services;

namespace LW4._2_Kovalchuk.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _service;

        public UsersController(IUsersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserItem>> GetById(int id)
        {
            try
            {
                var user = await _service.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Користувача з ID={id} не знайдено.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserItem>> Create(UserItem newUser)
        {
            try
            {
                var createdUser = await _service.CreateAsync(newUser);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
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
        public async Task<IActionResult> Update(int id, UserItem updatedUser)
        {
            try
            {
                await _service.UpdateAsync(id, updatedUser);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Користувача з ID={id} не знайдено.");
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
                return NotFound($"Користувача з ID={id} не знайдено.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("setRoles")]
        public async Task<IActionResult> SetRoles(string id, Roles roles)
        {
            var user = await _service.GetByIdAsync(int.Parse(id));
            if (user == null) return NotFound("User not found");

            user.Role = roles;
            await _service.UpdateAsync(int.Parse(id), user);

            return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(string id, Roles role)
        {
            var user = await _service.GetByIdAsync(int.Parse(id));
            if (user == null) return NotFound();

            user.Role |= role;
            await _service.UpdateAsync(int.Parse(id), user);

            return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
        }

        [HttpPost("removeRole")]
        public async Task<IActionResult> RemoveRole(string id, Roles role)
        {
            var user = await _service.GetByIdAsync(int.Parse(id));
            if (user == null) return NotFound();

            user.Role &= ~role;
            await _service.UpdateAsync(int.Parse(id), user);

            return Ok(new { username = user.Username, roles = user.Role, rolesValue = (int)user.Role });
        }

        [HttpGet("count-by-role/{role}")]
        public async Task<ActionResult<int>> GetCountByRole(Roles role)
        {
            try
            {
                var count = await _service.GetCountByRoleAsync(role);
                return Ok(count);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}