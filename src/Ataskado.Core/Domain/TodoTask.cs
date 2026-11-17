using System.Globalization;
using System.Text.RegularExpressions;

namespace Ataskado.Core.Domain;

public sealed partial class TodoTask
{
    #region Fields & Properties
    public bool IsCompleted { get; private set; }
    public char? Priority { get; private set; }
    public DateOnly? CompletedOn { get; private set; }
    public DateOnly? CreatedOn { get; private set; }
    #endregion

    #region Constructor
    private TodoTask() { }
    #endregion

    #region Methods
    public static Result<TodoTask> Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return TodoErrors.EmptyLine;

        Match match = PrefixesRegex().Match(line);

        (bool isCompleted, DateOnly? completedOn) = GetCompletion(match);

        TodoTask todoTask = new()
        {
            IsCompleted = isCompleted,
            CompletedOn = completedOn,
            Priority = GetPriority(match),
            CreatedOn = GetCreationDate(match)
        };

        return todoTask;
    }

    private static (bool, DateOnly?) GetCompletion(Match match)
    {
        bool isCompleted = match.Groups["done"].Success;
        DateOnly? completedOn = ParseDate(match.Groups["completed"]);

        return (isCompleted, completedOn);
    }

    private static DateOnly? GetCreationDate(Match match) =>
        ParseDate(match.Groups["created"]);

    private static char? GetPriority(Match match)
    {
        return match.Groups["priority"].Success
            ? match.Groups["priority"].Value[0]
            : null;
    }

    private static DateOnly? ParseDate(Group group)
    {
        if (!group.Success)
            return null;

        return DateOnly.TryParseExact(
            group.Value,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateOnly date) ? date : null;
    }
    #endregion

    #region Compiled Regex
    [GeneratedRegex(@"
            ^                                                                   # start
            (?:(?<done>x)[ ](?:(?<completed>[0-9]{4}-[0-9]{2}-[0-9]{2})[ ])?)?  # completion
            (?:\((?<priority>[A-Z])\)[ ])?                                      # priority
            (?:(?<created>[0-9]{4}-[0-9]{2}-[0-9]{2})[ ])?                      # creation
        ",
        RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex PrefixesRegex();
    #endregion
}
