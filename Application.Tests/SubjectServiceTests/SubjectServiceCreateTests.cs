using Application.DTO;
using Application.IPublisher;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.ValueObjects;
using Moq;

namespace Application.Tests.SubjectServiceTests;

public class SubjectServiceCreateTests
{
    [Fact]
    public async Task Create_WithValidInput_ShouldReturnSuccessResult()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var description = new Description("Math");
        var details = new Details("Intro to Algebra");

        var dto = new CreateSubjectDTO { Description = description.Value, Details = details.Value };
        var subject = new Mock<ISubject>();

        subject.Setup(s => s.Id).Returns(subjectId);
        subject.Setup(s => s.Description).Returns(description);
        subject.Setup(s => s.Details).Returns(details);

        var factoryMock = new Mock<ISubjectFactory>();
        factoryMock.Setup(f => f.Create(dto.Description, dto.Details)).ReturnsAsync(subject.Object);

        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.AddAsync(subject.Object)).ReturnsAsync(subject.Object);

        var publisherMock = new Mock<IMassTransitPublisher>();
        publisherMock.Setup(p => p.PublishCreatedSubjectCreatedMessage(
            subject.Object.Id, subject.Object.Description, subject.Object.Details)).Returns(Task.CompletedTask);

        var service = new SubjectService(factoryMock.Object, repoMock.Object, publisherMock.Object);

        // Act
        var result = await service.Create(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(subjectId, result.Value!.Id);
        Assert.Equal(description.Value, result.Value.Description);
        Assert.Equal(details.Value, result.Value.Details);
    }

    [Fact]
    public async Task Create_WhenFactoryThrows_ShouldReturnFailureResult()
    {
        // Arrange
        var dto = new CreateSubjectDTO { Description = "", Details = "Details" };

        var factoryMock = new Mock<ISubjectFactory>();
        factoryMock.Setup(f => f.Create(dto.Description, dto.Details))
                   .ThrowsAsync(new ArgumentException("Description can't be empty!"));

        var service = new SubjectService(factoryMock.Object,
            Mock.Of<ISubjectRepository>(),
            Mock.Of<IMassTransitPublisher>());

        // Act
        var result = await service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Description can't be empty!", result.Error!.Message);
    }

    [Fact]
    public async Task Create_WhenRepositoryReturnsNull_ShouldReturnFailure()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var description = new Description("Math");
        var details = new Details("Details");

        var dto = new CreateSubjectDTO { Description = description.Value, Details = details.Value };

        var subject = new Mock<ISubject>();
        subject.Setup(s => s.Id).Returns(subjectId);
        subject.Setup(s => s.Description).Returns(description);
        subject.Setup(s => s.Details).Returns(details);

        var factoryMock = new Mock<ISubjectFactory>();
        factoryMock.Setup(f => f.Create(dto.Description, dto.Details)).ReturnsAsync(subject.Object);

        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.AddAsync(subject.Object)).ReturnsAsync((ISubject?)null);

        var service = new SubjectService(factoryMock.Object, repoMock.Object, Mock.Of<IMassTransitPublisher>());

        // Act
        var result = await service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("DB error", result.Error!.Message);
    }

    [Fact]
    public async Task Create_WhenPublisherThrows_ShouldReturnFailure()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var description = new Description("Physics");
        var details = new Details("Quantum stuff");

        var dto = new CreateSubjectDTO { Description = description.Value, Details = details.Value };

        var subject = new Mock<ISubject>();
        subject.Setup(s => s.Id).Returns(subjectId);
        subject.Setup(s => s.Description).Returns(description);
        subject.Setup(s => s.Details).Returns(details);

        var factoryMock = new Mock<ISubjectFactory>();
        factoryMock.Setup(f => f.Create(dto.Description, dto.Details)).ReturnsAsync(subject.Object);

        var repoMock = new Mock<ISubjectRepository>();
        repoMock.Setup(r => r.AddAsync(subject.Object)).ReturnsAsync(subject.Object);

        var publisherMock = new Mock<IMassTransitPublisher>();
        publisherMock.Setup(p => p.PublishCreatedSubjectCreatedMessage(
            subject.Object.Id, subject.Object.Description, subject.Object.Details))
            .ThrowsAsync(new Exception("Broker error"));

        var service = new SubjectService(factoryMock.Object, repoMock.Object, publisherMock.Object);

        // Act
        var result = await service.Create(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Broker error", result.Error!.Message);
    }

}
