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
            return TaskErrors.EmptyLine;

        List<string> tokens = [.. line.Split()];
        if (tokens.Any(t => string.IsNullOrWhiteSpace(t)))
            return TaskErrors.InvalidLine;

        TodoTask todoTask = new();

        (bool isCompleted, DateOnly? completedOn) = GetCompletion(line);
        todoTask.IsCompleted = isCompleted;
        todoTask.CompletedOn = completedOn;

        todoTask.Priority = GetPriority(line);
        todoTask.CreatedOn = GetCreationDate(line);

        return todoTask;
    }

    private static (bool, DateOnly?) GetCompletion(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return (false, null);

        Match match = CompletionRegex().Match(line);

        bool isCompleted = match.Groups["done"].Success;
        DateOnly? completedOn = null;

        if (match.Groups["completed"].Success &&
            DateOnly.TryParseExact(
                match.Groups["completed"].Value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateOnly date))
        {
            completedOn = date;
        }

        return (isCompleted, completedOn);
    }

    private static DateOnly? GetCreationDate(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        Match match = CreationDateRegex().Match(line);
        if (!match.Groups["created"].Success)
            return null;

        return DateOnly.TryParseExact(
            match.Groups["created"].Value,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateOnly creationDate)
                ? creationDate
                : null;
    }

    private static char? GetPriority(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        Match match = PriorityRegex().Match(line);
        return match.Groups["priority"].Success
            ? Convert.ToChar(match.Groups["priority"].Value)
            : null;
    }
    #endregion

    #region Compiled Regex
    [GeneratedRegex(@"
            ^                                                       # start
            (?:(?<done>x)\s(?:(?<completed>\d{4}-\d{2}-\d{2})\s)?)? # completion
        ",
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex CompletionRegex();

    [GeneratedRegex(@"
            ^                                       # start
            (?:x\s(?:\d{4}-\d{2}-\d{2}\s)?)?        # completion
            (?:\([A-Z]\)\s)?                        # priority
            (?:(?<created>\d{4}-\d{2}-\d{2})\s)?    # creation
        ",
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]

    private static partial Regex CreationDateRegex();

    [GeneratedRegex(@"
            ^                                       # start
            (?:x\s(?:\d{4}-\d{2}-\d{2}\s)?)?        # completion
            (?:\((?<priority>[A-Z])\)\s)?           # priority
        ",
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace)]
    private static partial Regex PriorityRegex();
    #endregion
}