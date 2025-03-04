namespace TicketBookingSystem.Domain;

using System;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    // Protected constructor for use in derived classes.
    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && !string.IsNullOrEmpty(error))
            throw new InvalidOperationException("A successful result cannot have an error message.");
        if (!isSuccess && string.IsNullOrEmpty(error))
            throw new InvalidOperationException("A failure result must have an error message.");

        IsSuccess = isSuccess;
        Error = error;
    }

    // Factory method for a successful non-generic result.
    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    // Factory method for a failed non-generic result.
    public static Result Failure(string error)
    {
        return new Result(false, error);
    }
}

public class Result<T> : Result
{
    public T Value { get; }

    private Result(bool isSuccess, T value, string error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    // Factory method for a successful generic result.
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }

    // Factory method for a failed generic result.
    public new static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }
}
