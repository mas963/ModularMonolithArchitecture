using ModularMonolith.Modules.Customers.Application.Models;
using ModularMonolith.Shared.Abstractions.Commands;

namespace ModularMonolith.Modules.Customers.Application.Commands;

public class RegisterUserCommand : ICommand
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }   
}