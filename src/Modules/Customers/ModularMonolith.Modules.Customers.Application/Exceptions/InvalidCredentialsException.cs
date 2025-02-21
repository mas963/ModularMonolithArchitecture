namespace ModularMonolith.Modules.Customers.Application.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() 
        : base("Invalid email or password.")
    {
    }
}