using Domain.ValueObjects;

namespace Domain.Tests.DescriptionTests;

public class DescriptionConstructorTests
{
    [Theory]
    [InlineData("Math")]
    [InlineData("Physics is fun")]
    [InlineData("History and Culture")]
    public void WhenPassingValidDescriptionString_ThenInstantiateObject(string description)
    {
        // Act
        var result = new Description(description);
        
        //Assert
        Assert.Equal(description, result.Value);
    }

    [Fact]
    public void WhenPassingEmptyDescriptionString_ThenThrowException()
    {
        // Arrange
        string empty = "";

        // Assert
        var ex = Assert.Throws<ArgumentException>(() =>

            // Act
            new Description(empty)
        );

        Assert.Equal("Description can't be empty!", ex.Message);
    }

    [Fact]
    public void WhenPassingNullDescriptionString_ThenThrowException()
    {
        // Assert
        var ex = Assert.Throws<ArgumentException>(() => 

            // Act
            new Description(null!)
        );

        Assert.Equal("Description can't be empty!", ex.Message);
    }

    [Fact]
    public void WhenPassingDescriptionStringLargerThan50_ThenThrowException()
    {
        // Arrange
        var longDescription = new string('a', 51);
        
        // Assert
        var ex = Assert.Throws<ArgumentException>(() => 
            // Act
            new Description(longDescription)
        );
        Assert.Equal("Description has a max 50 characters!", ex.Message);
    }
}
