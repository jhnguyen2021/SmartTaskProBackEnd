using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Services.Interfaces;

namespace SmartTaskPro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwt;

    public AuthController(UserManager<User> userManager, IJwtTokenService jwt)
    {
        _userManager = userManager; _jwt = jwt;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest req)
    {
        var user = new User { Email = req.Email, FullName = req.FullName };
        var result = await _userManager.CreateAsync(user, req.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwt.CreateToken(user, roles);
        return Ok(new AuthResponse(user.Id.ToString(), user.Email!, token));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest req)
    {
        var user = await _userManager.FindByEmailAsync(req.Email);
        if (user is null) return Unauthorized();

        var check = await _userManager.CheckPasswordAsync(user, req.Password);
        if (!check) return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwt.CreateToken(user, roles);
        return Ok(new AuthResponse(user.Id.ToString(), user.Email!, token));
    }
}
