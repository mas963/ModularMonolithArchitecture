namespace ModularMonolith.Modules.Customers.Application.Models;

public class AuthResult
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; } = new();
}