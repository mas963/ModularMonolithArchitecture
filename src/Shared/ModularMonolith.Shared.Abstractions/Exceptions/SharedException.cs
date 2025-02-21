namespace ModularMonolith.Shared.Abstractions.Exceptions;

public abstract class SharedException : Exception
{
    protected SharedException(string message) : base(message)
    {
    }
}

public class NotFoundException : SharedException
{
    public NotFoundException(string message) : base(message)
    {
    }
}

public class ValidationException : SharedException
{
    public ValidationException(string message) : base(message)
    {
    }
}