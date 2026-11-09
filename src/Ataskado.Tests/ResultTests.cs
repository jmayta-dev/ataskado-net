using Xunit;
using Ataskado.Core.Domain;

namespace Ataskado.Tests;

public class ResultTests
{
    [Fact]
    public void Successful_result_is_success()
    {
        Result successfulResult = Result.Success();
        Assert.True(successfulResult.IsSuccess);
    }

    [Fact]
    public void An_error_result_is_unsuccessful()
    {
        Error error = new("Unknown.Error", "Unknown error message", ErrorType.NotFound);
        Result failedResult = (Result)error;
        Assert.False(failedResult.IsSuccess);
    }
}