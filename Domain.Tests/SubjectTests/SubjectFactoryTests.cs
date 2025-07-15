using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Moq;

namespace Domain.Tests.SubjectTests;

public class SubjectFactoryTests
{
    [Fact]
    public async Task Create_WhenSubjectWithDescriptionDoesNotExist_ShouldReturnNewSubject()
    {
        // Arrange
        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.GetSubjectByDescription("Math"))
                .ReturnsAsync((ISubject?)null);

        var factory = new SubjectFactory(repoMock.Object);

        // Act
        var result = await factory.Create("Math", "Basic math skills");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Math", result.Description.Value);
        Assert.Equal("Basic math skills", result.Details.Value);
        Assert.IsType<Subject>(result);

        repoMock.Verify(r => r.GetSubjectByDescription("Math"), Times.Once);
    }

    [Fact]
    public async Task Create_WhenSubjectWithDescriptionExists_ShouldThrowArgumentException()
    {
        // Arrange
        var existingSubjectMock = new Mock<ISubject>();
        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.GetSubjectByDescription("Math"))
                .ReturnsAsync(existingSubjectMock.Object);

        var factory = new SubjectFactory(repoMock.Object);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            factory.Create("Math", "Another detail"));

        Assert.Equal("The subject's description already exists!", ex.Message);
    }

    [Fact]
    public async Task Create_WhenDescriptionIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.GetSubjectByDescription(""))
                .ReturnsAsync((ISubject?)null);

        var factory = new SubjectFactory(repoMock.Object);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            factory.Create("", "Some details"));

        Assert.Equal("Description can't be empty!", ex.Message);
    }

    [Fact]
    public async Task Create_WhenDetailsIsInvalid_ShouldThrowArgumentException()
    {
        // Arrange
        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.GetSubjectByDescription("Math"))
                .ReturnsAsync((ISubject?)null);

        var longDetails = new string('x', 501);
        var factory = new SubjectFactory(repoMock.Object);

        // Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() =>
            // Act
            factory.Create("Math", longDetails));

        Assert.Equal("Details has a max 500 characters!", ex.Message);
    }

    [Fact]
    public void ShouldReturnSubjectWithCorrectValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Valid description";
        var details = "Some details";

        var repoMock = new Mock<ISubjectRepository>();
        var factory = new SubjectFactory(repoMock.Object);

        // Act
        var result = factory.Create(id, description, details);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description.Value);
        Assert.Equal(details, result.Details.Value);
    }
}

