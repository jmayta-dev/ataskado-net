namespace Ataskado.Core.Domain;

public static class TaskErrors
{
    public static Error EmptyLine => new(
        "TaskErrors.EmptyLine",
        "The task cannot be an empty line.",
        ErrorType.Validation);
}
