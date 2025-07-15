using Application.IPublisher;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Moq;

namespace Application.Tests.SubjectServiceTests;
public class SubjectServiceCreateWithNoValidationTests
{
    [Fact]
    public async Task ShouldAddSubject_WhenSubjectDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = "Test Subject";
        var details = "Test Details";

        var _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((ISubject?)null);

        var subjectMock = new Mock<ISubject>();
        var _subjectFactoryMock = new Mock<ISubjectFactory>();
        _subjectFactoryMock.Setup(f => f.Create(id, description, details)).Returns(subjectMock.Object);

        var publisherMock = new Mock<IMassTransitPublisher>();

        var service = new SubjectService(_subjectFactoryMock.Object, _subjectRepositoryMock.Object, publisherMock.Object);

        // Act
        await service.CreateWithNoValidation(id, description, details);

        // Assert
        _subjectFactoryMock.Verify(f => f.Create(id, description, details), Times.Once);
        _subjectRepositoryMock.Verify(r => r.AddAsync(subjectMock.Object), Times.Once);
    }

    [Fact]
    public async Task ShouldDoNothing_WhenSubjectAlreadyExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var existingSubjectMock = new Mock<ISubject>();

        var _subjectRepositoryMock = new Mock<ISubjectRepository>();
        _subjectRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existingSubjectMock.Object);

        var _subjectFactoryMock = new Mock<ISubjectFactory>();
        var publisherMock = new Mock<IMassTransitPublisher>();

        var service = new SubjectService(_subjectFactoryMock.Object, _subjectRepositoryMock.Object, publisherMock.Object);

        // Act
        await service.CreateWithNoValidation(id, "any", "any");

        // Assert
        _subjectFactoryMock.Verify(f => f.Create(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _subjectRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ISubject>()), Times.Never);
    }
}

