using Microsoft.AspNetCore.Identity;
using ModularMonolith.Modules.Customers.Application.Commands;
using ModularMonolith.Modules.Customers.Application.Exceptions;
using ModularMonolith.Modules.Customers.Application.Interfaces;
using ModularMonolith.Modules.Customers.Application.Models;
using ModularMonolith.Modules.Customers.Domain.Entities;
using ModularMonolith.Shared.Infrastructure.Auth;

namespace ModularMonolith.Modules.Customers.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Customer> _userManager;
    private readonly SignInManager<Customer> _signInManager;
    private readonly JwtProvider _jwtProvider;
    private readonly IEmailService _emailService;

    public IdentityService(UserManager<Customer> userManager, SignInManager<Customer> signInManager,
        JwtProvider jwtProvider, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _emailService = emailService;
    }

    public async Task<AuthResult> RegisterAsync(RegisterUserCommand command)
    {
        var user = Customer.Create(
            command.FirstName,
            command.LastName,
            command.Email);

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors.Select(e => e.Description).ToList());
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        await _emailService.SendEmailAsync(command.Email, "Welcome!", "Welcome to Modular Monolith!");

        return await GenerateAuthResultAsync(user);
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var customer = await _userManager.FindByEmailAsync(email);
        if (customer == null)
            throw new InvalidCredentialsException();

        if (!customer.IsActive)
            throw new UserDeactivatedException(customer.Id);

        var result = await _signInManager.CheckPasswordSignInAsync(customer, password, false);
        if (!result.Succeeded)
            throw new InvalidCredentialsException();

        return await GenerateAuthResultAsync(customer);
    }

    public async Task<bool> ChangePasswordAsync(Guid customerId, string currentPassword, string newPassword)
    {
        var customer = await _userManager.FindByIdAsync(customerId.ToString());
        if (customer == null)
            throw new InvalidCredentialsException();

        var result = await _userManager.ChangePasswordAsync(customer, currentPassword, newPassword);
        if (!result.Succeeded)
            throw new InvalidCredentialsException();

        return result.Succeeded;
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var customer = await _userManager.FindByEmailAsync(email);
        if (customer == null)
            throw new InvalidCredentialsException();

        var result = await _userManager.ResetPasswordAsync(customer, token, newPassword);
        if (!result.Succeeded)
            throw new InvalidCredentialsException();

        return result.Succeeded;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var customer = await _userManager.FindByEmailAsync(email);
        if (customer == null)
            throw new InvalidCredentialsException();

        var token = await _userManager.GeneratePasswordResetTokenAsync(customer);
        return token;
    }

    private async Task<AuthResult> GenerateAuthResultAsync(Customer customer)
    {
        var roles = await _userManager.GetRolesAsync(customer);
        var token = _jwtProvider.GenerateToken(customer.Id, customer.Email, roles);

        return new AuthResult
        {
            Token = token,
            UserId = customer.Id,
            Email = customer.Email,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Roles = roles.ToList()
        };
    }
}