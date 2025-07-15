using Application;
using Microsoft.AspNetCore.Mvc;

namespace InterfaceAdapters.ResultExtensionsTests;

public class ResultExtensionsTests
{
    [Fact]
    public void ToActionResult_Generic_Success_ReturnsOkObjectWithValue()
    {
        // Arrange
        var value = "hello";
        var result = Result<string>.Success(value);

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var okObject = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(value, okObject.Value);
    }

    [Fact]
    public void ToActionResult_Generic_Failure_ReturnsBadRequestResult()
    {
        // Arrange
        var result = Result<string>.Failure(Error.BadRequest("Invalid"));

        // Act
        var actionResult = result.ToActionResult();

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal("Invalid", badRequest.Value);
    }

    [Fact]
    public void ToActionResult_Generic_Failure_ReturnsUnauthorizedResult()
    {
        var result = Result<string>.Failure(Error.Unauthorized("Unauthorized access"));

        var actionResult = result.ToActionResult();

        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(actionResult.Result);
        Assert.Equal("Unauthorized access", unauthorized.Value);
    }

    [Fact]
    public void ToActionResult_Generic_Failure_ReturnsForbiddenResult()
    {
        var result = Result<string>.Failure(Error.Forbidden("Forbidden"));

        var actionResult = result.ToActionResult();

        var objResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal("Forbidden", objResult.Value);
        Assert.Equal(403, objResult.StatusCode);
    }

    [Fact]
    public void ToActionResult_Generic_Failure_ReturnsNotFoundResult()
    {
        var result = Result<string>.Failure(Error.NotFound("Not found"));

        var actionResult = result.ToActionResult();

        var notFound = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        Assert.Equal("Not found", notFound.Value);
    }

    [Fact]
    public void ToActionResult_Generic_Failure_ReturnsInternalServerErrorResult()
    {
        var result = Result<string>.Failure(Error.InternalServerError("Server error"));

        var actionResult = result.ToActionResult();

        var objResult = Assert.IsType<ObjectResult>(actionResult.Result);
        Assert.Equal("Server error", objResult.Value);
        Assert.Equal(500, objResult.StatusCode);
    }
}
