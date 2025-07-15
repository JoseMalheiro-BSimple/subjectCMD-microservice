using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Moq;

namespace Infrastructure.Tests.DataModelTests;
public class SubjectDataModelConstructorTests
{

    [Fact]
    public void WhenPassingValidSubject_ThenInstatiateDataModel()
    {
        // Arrange
        var subject = new Mock<ISubject>();

        var id = Guid.NewGuid();
        var description = new Description("Test description");
        var details = new Details("Test details");

        subject.Setup(s => s.Id).Returns(id);
        subject.Setup(s => s.Description).Returns(description);
        subject.Setup(s => s.Details).Returns(details);

        // Act
        SubjectDataModel result = new SubjectDataModel(subject.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
        Assert.Equal(details, result.Details);
    }
}
