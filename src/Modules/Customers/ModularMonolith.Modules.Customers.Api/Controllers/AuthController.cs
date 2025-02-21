using Microsoft.AspNetCore.Mvc;
using ModularMonolith.Modules.Customers.Api.Request;
using ModularMonolith.Modules.Customers.Application.Commands;
using ModularMonolith.Modules.Customers.Application.Interfaces;

namespace ModularMonolith.Modules.Customers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        var result = await _identityService.RegisterAsync(command);

        var response = new AuthResponse
        {
            Token = result.Token,
            UserId = result.UserId,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Roles = result.Roles
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var result = await _identityService.LoginAsync(request.Email, request.Password);

        var response = new AuthResponse
        {
            Token = result.Token,
            UserId = result.UserId,
            Email = result.Email,
            FirstName = result.FirstName,
            LastName = result.LastName,
            Roles = result.Roles
        };

        return Ok(response);
    }
}