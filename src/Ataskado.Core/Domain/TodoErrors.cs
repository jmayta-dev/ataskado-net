namespace Ataskado.Core.Domain;

public static class TodoErrors
{
    public static Error EmptyLine => new(
        "TodoErrors.EmptyLine",
        "The task cannot be an empty line.",
        ErrorType.Validation);
}
