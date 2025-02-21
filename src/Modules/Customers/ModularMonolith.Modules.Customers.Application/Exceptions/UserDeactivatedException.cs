namespace ModularMonolith.Modules.Customers.Application.Exceptions;

public class UserDeactivatedException : Exception
{
    public Guid UserId { get; }

    public UserDeactivatedException(Guid userId) 
        : base($"User with ID {userId} is deactivated.")
    {
        UserId = userId;
    }
}