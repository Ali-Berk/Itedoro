namespace Itedoro.Application.Common.Shared.Results;
public class Result
{
    private readonly Error _error;
    private readonly bool _isSuccess;

    public bool IsSuccess => _isSuccess;
    public bool IsFailure => !_isSuccess;
    public Error Error => _error;

    protected Result()
    {
        _isSuccess = true;
        _error = Error.None;
    }

    protected Result(Error error)
    {
        _isSuccess = false;
        _error = error;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);

    public static implicit operator Result(Error error) => new(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue? Value => _value;

    private Result(TValue value) : base()
    {
        _value = value;
    }

    private Result(Error error) : base(error)
    {
        _value = default;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public static new Result<TValue> Failure(Error error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => new(value);
    public static implicit operator Result<TValue>(Error error) => new(error);
}

public record class Error(string Code, string Description)
{
    public static Error None => new(String.Empty, String.Empty);
}