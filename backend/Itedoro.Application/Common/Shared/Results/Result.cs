namespace Itedoro.Application.Common.Shared.Result;

public class Result
{
    protected bool _success;
    public IEnumerable<string> Errors { get; protected set; } = [];

    public bool IsSuccess => _success;
    public bool IsFailure => !_success;

    public static Result Success() => new() { _success = true };
    public static Result Failure(string[] errors) => new() { _success = false, Errors = errors };
    public static Result Failure(string error) => new() { _success = false, Errors = new[] { error } };
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    public static Result<T> Success(T data) => new() { _success = true, Value = data };
    public static new Result<T> Failure(string[] errors) => new() { _success = false, Errors = errors };
    public static new Result<T> Failure(string error) => new() { _success = false, Errors = new[] { error } };
}