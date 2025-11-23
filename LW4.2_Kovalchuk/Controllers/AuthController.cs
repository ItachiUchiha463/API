using Microsoft.AspNetCore.Mvc;
using LW4._2_Kovalchuk.Models;
using LW4._2_Kovalchuk.Services;
using LW4._2_Kovalchuk.Interfaces;
using MongoAuthApi.Helpers;
using System.Security.Claims;

namespace LW4._2_Kovalchuk.Controller;

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsersService _userService;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(IUsersService userService, JwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserItem user)
    {
        var existingUser = await _userService.GetByEmailAsync(user.Email);
        if (existingUser != null)
            return BadRequest("Користувач уже існує.");

        user.Password = _passwordHasher.HashPassword(user.Password);
        await _userService.CreateAsync(user);

        return Ok("Користувач зареєстрований.");
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userService.GetByEmailAsync(model.Email);
        if (user == null)
            return Unauthorized("Неправильна пошта або пароль.");
        if (!_passwordHasher.Verify(model.Password, user.Password))
            return Unauthorized("Неправильна пошта або пароль.");
        var accessToken = _jwtTokenGenerator.Generate(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userService.UpdateAsync(user.Id!.Value, user); 
        return Ok(new
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            TokenExpiryTime = DateTime.UtcNow.AddMinutes(60),
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromHeader(Name = "Authorization")] string authHeader, [FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return BadRequest("Missing or invalid Authorization header");

        var token = authHeader.Substring("Bearer ".Length);

        var principal = _jwtTokenGenerator.GetPrincipalFromExpiredToken(token);

        if (principal == null)
            return BadRequest("Invalid refresh token");

        var id = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (id == null) throw new Exception("Invalid token claims");

        var user = await _userService.GetByIdAsync(int.Parse(id));

        if (user == null ||
            user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return Unauthorized("Invalid refresh token");
        }

        var newAccessToken = _jwtTokenGenerator.Generate(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _userService.UpdateAsync(int.Parse(id),user);

        return Ok(new
        {
            token = newAccessToken,
            refreshToken = newRefreshToken,
            TokenExpiryTime = DateTime.UtcNow.AddMinutes(60),
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime
        });
    }
  
}