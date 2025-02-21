using ModularMonolith.Modules.Customers.Application.Commands;
using ModularMonolith.Modules.Customers.Application.Models;

namespace ModularMonolith.Modules.Customers.Application.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> RegisterAsync(RegisterUserCommand command);
    Task<AuthResult> LoginAsync(string email, string password);
    Task<bool> ChangePasswordAsync(Guid customerId, string currentPassword, string newPassword);
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
    Task<string> GeneratePasswordResetTokenAsync(string email);
}