namespace Ataskado.Core.Domain;

public enum ErrorType
{
    NotFound,
    Conflict,
    Validation
}

public sealed record Error(string Code, string Description, ErrorType Type);

public class Result
{
    public Error? Error { get; }
    public bool IsSuccess => Error is null;

    #region Constructors
    protected Result(Error? error) => Error = error;
    #endregion

    #region Methods
    public static Result Success() => new(null);
    #endregion

    #region Implicit Operators
    public static implicit operator Result(Error error) => new(error);
    #endregion
}

public sealed class Result<TValue> : Result
{
    #region Fields & Properties
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("A failed result has no value.");
    #endregion

    #region Constructors
    private Result(TValue? value, Error? error)
        : base(error) => _value = value;
    #endregion

    #region Implicit Operators
    public static implicit operator Result<TValue>(TValue value) =>
        new(value, null);

    public static implicit operator Result<TValue>(Error error) =>
        new(default, error);
    #endregion
}