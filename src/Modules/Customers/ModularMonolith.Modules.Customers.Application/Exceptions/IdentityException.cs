namespace ModularMonolith.Modules.Customers.Application.Exceptions;

public class IdentityException : Exception
{
    public List<string> Errors { get; }

    public IdentityException(List<string> errors)
        : base("One or more identity errors occurred.")
    {
        Errors = errors;
    }
}