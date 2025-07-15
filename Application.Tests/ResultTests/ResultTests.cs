namespace Application.Tests.ResultTests;
public class ResultTests
{
    [Fact]
    public void Success_ShouldHaveIsSuccessTrue_AndIsFailureFalse_AndNoError()
    {
        // Arrange
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ShouldHaveIsSuccessFalse_AndIsFailureTrue_AndErrorSet()
    {
        // Arrange
        var errorMessage = "Something went wrong";
        var error = Error.BadRequest(errorMessage);

        var result = Result.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(errorMessage, result.Error!.Message);
        Assert.Equal(400, result.Error.StatusCode);
    }

    [Fact]
    public void GenericSuccess_ShouldContainValue_AndIsSuccessTrue()
    {
        // Arrange
        var value = 42;
        var result = Result<int>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(value, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void GenericFailure_ShouldHaveIsSuccessFalse_AndErrorSet()
    {
        // Arrange
        var errorMessage = "Invalid input";
        var error = Error.BadRequest(errorMessage);

        var result = Result<int>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(errorMessage, result.Error!.Message);
        Assert.Equal(400, result.Error.StatusCode);
        Assert.Equal(default(int), result.Value); // Value should be 0
    }
}
