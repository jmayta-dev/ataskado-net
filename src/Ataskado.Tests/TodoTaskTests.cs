using System.Globalization;
using Ataskado.Core.Domain;

namespace Ataskado.Tests;

public class TodoTaskTests
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void An_empty_line_returns_an_error(string line)
    {
        // Arrange
        Error expectedError = TaskErrors.EmptyLine;
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);
        // Assert
        Assert.Equal(expectedError, result.Error);
    }

    [Theory]
    [InlineData("x 2011-03-02 2011-03-01 Review", true)]
    [InlineData("xylophone lesson", false)]
    [InlineData("X 2026-01-01 Make resolutions", false)]
    [InlineData("(A) x Find ticket prices", false)]
    [InlineData("x task without completion date", true)]
    public void A_task_is_completed_only_when_it_starts_with_a_lowercase_x(
        string line,
        bool isCompleted)
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);

        // Assert
        Assert.Equal(isCompleted, result.Value.IsCompleted);
    }

    [Theory]
    [InlineData("x 2012-01-01 make resolutions", "2012-01-01")]
    [InlineData("x 2011-03-02 2011-03-01 Review", "2011-03-02")]
    public void A_completion_date_after_a_leading_lowercase_x_is_recorded(
        string line,
        string stringDate)
    {
        // Arrange
        DateOnly expectedCompletionDate = DateOnly.ParseExact(
            stringDate,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);
        // Assert
        Assert.Equal(expectedCompletionDate, result.Value.CompletedOn);
    }

    [Fact]
    public void A_completion_mark_after_the_first_token_does_not_set_a_completion_date()
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(
            "2026-01-01 x 2026-01-02 some random task");
        // Assert
        Assert.False(result.Value.IsCompleted);
        Assert.Null(result.Value.CompletedOn);
    }

    [Fact]
    public void A_task_without_a_completion_mark_has_no_completion_date()
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(
            "2026-01-01 2026-01-02 some random task");
        // Assert
        Assert.Null(result.Value.CompletedOn);
    }

    [Theory]
    [InlineData("(A) Call Mom", 'A')]
    [InlineData("(B) x Find ticket prices", 'B')]
    [InlineData("(Z) 2011-03-01 Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", 'Z')]
    [InlineData("x (A) Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", 'A')]
    [InlineData("x 2011-03-02 (A) Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", 'A')]
    public void A_task_with_a_parenthesized_capital_letter_has_that_priority(
        string line,
        char? expectedPriority)
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);

        // Assert
        Assert.Equal(expectedPriority, result.Value.Priority);
    }

    [Theory]
    [InlineData("Really gotta call Mom (A) @phone @someday")]
    [InlineData("(a) foo")]
    [InlineData("(B)->Submit TPS report")]
    [InlineData("(A)Call Mom")]
    [InlineData("(b) Get back to the boss")]
    public void A_task_without_a_valid_leading_priority_has_no_priority(string line)
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);

        // Assert
        Assert.Null(result.Value.Priority);
    }

    [Theory]
    [InlineData("2011-03-02 Document +TodoTxt task format", "2011-03-02")]
    [InlineData("(A) 2011-03-02 Call Mom", "2011-03-02")]
    public void A_date_at_the_start_of_a_task_is_its_creation_date(
        string line,
        string stringDate)
    {
        // Arrange
        DateOnly expectedCreationDate = DateOnly.ParseExact(
            stringDate,
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);
        // Assert
        Assert.Equal(expectedCreationDate, result.Value.CreatedOn);
    }

    [Fact]
    public void A_completed_task_with_two_dates_was_created_on_the_second_one()
    {
        // Arrange
        DateOnly expectedCreationDate = DateOnly.ParseExact(
            "2011-03-01",
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture);
        // Act
        Result<TodoTask> result = TodoTask.Parse("x 2011-03-02 2011-03-01 Review");
        // Assert
        Assert.Equal(expectedCreationDate, result.Value.CreatedOn);
    }

    [Fact]
    public void A_date_after_the_description_is_not_a_creation_date()
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse("(A) Call Mom 2011-03-02");
        // Assert
        Assert.Null(result.Value.CreatedOn);
    }
}
