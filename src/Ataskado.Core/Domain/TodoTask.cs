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

        TodoTask todoTask = new();

        Match match = PrefixesRegex().Match(line);
        //
        // get task prefixes
        //

        // completion state
        todoTask.IsCompleted = match.Groups["done"].Success;

        // completion date
        if (match.Groups["completed"].Success)
        {
            todoTask.CompletedOn = ParseDate(match.Groups["completed"]);
            if (todoTask.CompletedOn == null)
                return TodoErrors.InvalidCompletionDate;
        }

        // priority
        if (match.Groups["priority"].Success)
            todoTask.Priority = match.Groups["priority"].Value[0];

        // creation date
        if (match.Groups["created"].Success)
        {
            todoTask.CreatedOn = ParseDate(match.Groups["created"]);
            if (todoTask.CreatedOn == null)
                return TodoErrors.InvalidCreationDate;
        }

        return todoTask;
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
        RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture)]
    private static partial Regex PrefixesRegex();
    #endregion
}
