using System.Collections.ObjectModel;

namespace Application.Model;


public class Result<TResponse>(bool isSuccess)
{
    private readonly IList<string> _messages = [];

    public bool IsSuccess { get; } = isSuccess;
    public bool IsFailure => !IsSuccess;
    public IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);
    public TResponse? Data { get; private set; }

    public static Result<TResponse> Success(TResponse data) => new(true) { Data = data };

    public static implicit operator Result<TResponse>(TResponse value) => Success(value);

    public Result<U> SetResult<U>() => Result<U>.Failure(Errors);

    public Result<U> SetResult<U>(Func<TResponse, U> func)
    {
        if (IsSuccess)
        {
            Result<U> result = func(Data!);
            return result;
        }

        return Result<U>.Failure(Errors);
    }

    public static Result<TResponse> Failure(string message)
        => new Result<TResponse>(false).AddError(message);

    public static Result<TResponse> Failure(IEnumerable<string> messages)
        => new Result<TResponse>(false).AddError(messages);

    private Result<TResponse> AddError(string message)
    {
        if (!_messages.Contains(message))
            _messages.Add(message);

        return this;
    }

    private Result<TResponse> AddError(
        IEnumerable<string> messages
    )
    {
        foreach (string message in messages)
            AddError(message);

        return this;
    }

    public override string ToString() => IsSuccess
        ? "Success"
        : string.Join("; ", _messages);
}

