namespace Itedoro.Domain.Exceptions;

public sealed class DomainException(string message) : Exception(message)
{
    public static string ThrowIfNullOrWhiteSpace(string? value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{parameterName} cannot be empty.");
        }

        return value.Trim();
    }

    public static Guid ThrowIfEmpty(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new DomainException($"{parameterName} cannot be empty.");
        }

        return value;
    }

    public static int ThrowIfNonPositive(int value, string parameterName)
    {
        if (value <= 0)
        {
            throw new DomainException($"{parameterName} must be greater than zero.");
        }

        return value;
    }

    public static void ThrowIf(bool condition, string message)
    {
        if (condition)
        {
            throw new DomainException(message);
        }
    }
}
