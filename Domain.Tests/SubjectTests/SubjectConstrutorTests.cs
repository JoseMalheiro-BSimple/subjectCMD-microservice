using Domain.Models;
using Domain.ValueObjects;

namespace Domain.Tests.SubjectTests;

public class SubjectConstrutorTests
{
    [Fact]
    public void WhenPassingValidArguments_ThenSubjectIsCreated()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = new Description("Math");
        var details = new Details("Basic algebra and geometry");

        // Act
        var subject = new Subject(id, description, details);

        // Assert
        Assert.Equal(id, subject.Id);
        Assert.Equal(description, subject.Description);
        Assert.Equal(details, subject.Details);
    }
}
