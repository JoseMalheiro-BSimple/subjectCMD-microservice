using Application.IPublisher;
using Application.Services;
using Domain.Factory;
using Domain.IRepository;
using Moq;

namespace Application.Tests.SubjectServiceTests;

public class SubjectServiceConstructorTests
{
    [Fact]
    public void Constructor_WithValidDependencies_ShouldCreateInstance()
    {
        // Arrange
        var factoryMock = new Mock<ISubjectFactory>();
        var repoMock = new Mock<ISubjectRepository>();
        var publisherMock = new Mock<IMassTransitPublisher>();

        // Act
        var service = new SubjectService(factoryMock.Object, repoMock.Object, publisherMock.Object);

        // Assert
        Assert.NotNull(service);
        Assert.IsType<SubjectService>(service);
    }
}

