using LW4._2_Kovalchuk.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    [Authorize(Roles = nameof(Roles.Admin))]
    [HttpGet("admin")]
    public IActionResult OnlyAdmins() => Ok("Admin access granted");

    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    [HttpGet("management")]
    public IActionResult ManagersAndAdmins() => Ok("Manager/Admin access granted");

    [Authorize]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        var name = User.Identity?.Name;
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);
        return Ok(new { name, roles });
    }
}