using Microsoft.AspNetCore.Identity;

namespace ModularMonolith.Modules.Customers.Domain.Entities;

public class Customer : IdentityUser<Guid>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private Customer() { } // For EF Core

    private Customer(string firstName, string lastName, string email)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = email;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public static Customer Create(string firstName, string lastName, string email)
    {
        return new Customer(firstName, lastName, email);
    }
}
