using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.SubjectRepositoryTests;

public class SubjectRepositoryAddTests : RepositoryTestBase
{
    [Fact]
    public void Add_ShouldAddSubjectAndReturnMappedSubject()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = new Description("Test Subject");
        var details = new Details("");

        // Properly mock domain subject
        var domainSubjectMock = new Mock<ISubject>();
        domainSubjectMock.SetupGet(s => s.Id).Returns(id);
        domainSubjectMock.SetupGet(s => s.Description).Returns(description);
        domainSubjectMock.SetupGet(s => s.Details).Returns(details);
        var domainSubject = domainSubjectMock.Object;

        var dataModel = new SubjectDataModel
        {
            Id = id,
            Description = description,
            Details = details
        };

        _mapper.Setup(m => m.Map<ISubject, SubjectDataModel>(domainSubject)).Returns(dataModel);
        _mapper.Setup(m => m.Map<SubjectDataModel, ISubject>(It.IsAny<SubjectDataModel>())).Returns(domainSubject);

        var repository = new SubjectRepository(context, _mapper.Object);

        // Act
        var result = repository.Add(domainSubject);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
        Assert.Equal(details, result.Details);

        _mapper.Verify(m => m.Map<ISubject, SubjectDataModel>(domainSubject), Times.Once);
        _mapper.Verify(m => m.Map<SubjectDataModel, ISubject>(It.IsAny<SubjectDataModel>()), Times.Once);
    }

    [Fact]
    public void Add_ShouldPersistEntityToDatabase()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = new Description("Persist Subject");
        var details = new Details("D");

        var domainSubject = new Mock<ISubject>();
        domainSubject.SetupGet(s => s.Id).Returns(id);
        domainSubject.SetupGet(s => s.Description).Returns(description);
        domainSubject.SetupGet(s => s.Details).Returns(details);

        var dataModel = new SubjectDataModel
        {
            Id = id,
            Description = description,
            Details = details
        };

        _mapper.Setup(m => m.Map<ISubject, SubjectDataModel>(domainSubject.Object)).Returns(dataModel);
        _mapper.Setup(m => m.Map<SubjectDataModel, ISubject>(It.IsAny<SubjectDataModel>())).Returns(domainSubject.Object);

        var repository = new SubjectRepository(context, _mapper.Object);

        // Act
        repository.Add(domainSubject.Object);

        // Assert
        var entityInDb = context.Subjects.FirstOrDefault(s => s.Id == id);
        Assert.NotNull(entityInDb);
        Assert.Equal("Persist Subject", entityInDb.Description.Value);
    }

    [Fact]
    public void Add_ShouldThrow_WhenInputIsNull()
    {
        // Arrange
        var repository = new SubjectRepository(context, _mapper.Object);

        // Assert
        Assert.Throws<NullReferenceException>(() =>
            // Act 
            repository.Add(null!)
        );
    }
}

