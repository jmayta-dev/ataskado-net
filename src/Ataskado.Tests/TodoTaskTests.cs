using Ataskado.Core.Domain;

namespace Ataskado.Tests;

public class TodoTaskTests
{
    [Fact]
    public void An_empty_line_returns_an_error()
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse("");
        // Assert
        Assert.False(result.IsSuccess);
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

    [Fact]
    public void A_completion_date_after_a_leading_lowercase_x_is_recorded()
    {
        // Arrange
        DateOnly expectedCompletionDate = new(2012, 1, 1);
        // Act
        Result<TodoTask> result = TodoTask.Parse(
            "x 2012-01-01 make resolutions");
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
    [InlineData("(A) Call Mom", "A")]
    [InlineData("(A) x Find ticket prices", "A")]
    [InlineData("(A) 2011-03-01 Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", "A")]
    [InlineData("x (A) Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", "A")]
    [InlineData("x 2011-03-02 (A) Review Tim's pull request +TodoTxtTouch @github due:2026-11-06", "A")]
    public void A_task_with_a_parenthesized_capital_letter_has_that_priority(
        string line,
        string priorityExpected)
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);

        // Assert
        Assert.Equal(priorityExpected, result.Value.Priority);
    }

    [Theory]
    [InlineData("Really gotta call Mom (A) @phone @someday")]
    [InlineData("(a) foo")]
    [InlineData("(B)->Submit TPS report")]
    public void A_task_without_a_valid_leading_priority_has_no_priority(string line)
    {
        // Arrange
        // Act
        Result<TodoTask> result = TodoTask.Parse(line);

        // Assert
        Assert.Empty(result.Value.Priority);
    }
}