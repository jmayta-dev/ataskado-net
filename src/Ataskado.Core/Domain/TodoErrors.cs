namespace Ataskado.Core.Domain;

public static class TodoErrors
{
    public static Error EmptyLine => new(
        "TodoErrors.EmptyLine",
        "The task cannot be an empty line.",
        ErrorType.Validation
    );

    public static Error InvalidCompletionDate => new(
        "TodoErrors.InvalidCompletionDate",
        "The completion date is not a valid date.",
        ErrorType.Validation
    );

    public static Error InvalidCreationDate => new(
        "TodoErrors.InvalidCreationDate",
        "The creation date is not a valid date.",
        ErrorType.Validation
    );
}
