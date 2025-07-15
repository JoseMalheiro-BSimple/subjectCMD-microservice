using Domain.ValueObjects;

namespace Domain.Tests.DetailsTests;

public class DetailsConstructorTests
{
    [Fact]
    public void WhenUsingEmptyConstructor_ThenInstatiateWithDefaultString()
    {
        // Act
        var details = new Details();

        // Assert
        Assert.NotNull(details);
        Assert.Equal("", details.Value);
    }

    [Theory]
    [InlineData("This is a short detail.")]
    [InlineData("More detailed information about the subject.")]
    [InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit.")]
    public void WhenPassingValidDetailsString_ThenInstantiateObject(string value)
    {
        // Act
        var result = new Details(value);

        // Assert
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void WhenPassingDescriptionStringWith500Characters_ThenInstantiateObject()
    {
        // Arrange
        var details = new string('a', 500);

        // Act
        var result = new Details(details);

        //Assert
        Assert.Equal(details, result.Value);
    }

    [Fact]
    public void WhenPassingNullDetailsString_ThenThrowArgumentNullException()
    {
        // Assert
        var ex = Assert.Throws<ArgumentNullException>(() => 
            // Act
            new Details(null!)
        );

        Assert.Equal("Description can't be null!", ex.ParamName); // See note below
    }

    [Fact]
    public void WhenPassingDetailsStringLargerThan500_ThenThrowArgumentException()
    {
        // Arrange
        var longValue = new string('a', 501);

        // Assert
        var ex = Assert.Throws<ArgumentException>(() => 
            // Act
            new Details(longValue));

        Assert.Equal("Details has a max 500 characters!", ex.Message);
    }
}
