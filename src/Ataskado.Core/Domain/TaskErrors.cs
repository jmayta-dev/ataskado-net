namespace Ataskado.Core.Domain;

public static class TaskErrors
{
    public static Error EmptyLine => new(
        "TaskError.EmptyLine",
        "The task cannot be an empty line.",
        ErrorType.Validation);

    public static Error ManySpaces => new(
        "TaskError.ManySpaces",
        "The task contains multiple consecutive spaces.",
        ErrorType.Validation);

    public static Error InvalidLine => new(
        "TaskError.InvalidLine",
        "The task contains invalid formatting.",
        ErrorType.Validation);
}