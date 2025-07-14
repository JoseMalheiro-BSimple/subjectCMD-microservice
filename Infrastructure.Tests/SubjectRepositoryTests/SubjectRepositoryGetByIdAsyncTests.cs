using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;
using Moq;

namespace Infrastructure.Tests.SubjectRepositoryTests;

public class SubjectRepositoryGetByIdAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedSubject_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var description = new Description("Async Subject");
        var details = new Details("Async Details");

        var subjectDM = new SubjectDataModel
        {
            Id = id,
            Description = description,
            Details = details
        };

        await context.Subjects.AddAsync(subjectDM);
        await context.SaveChangesAsync();

        var domainSubject = new Mock<ISubject>();
        domainSubject.SetupGet(s => s.Id).Returns(id);
        domainSubject.SetupGet(s => s.Description).Returns(description);
        domainSubject.SetupGet(s => s.Details).Returns(details);

        _mapper.Setup(m => m.Map<SubjectDataModel, ISubject>(subjectDM)).Returns(domainSubject.Object);

        var repository = new SubjectRepository(context, _mapper.Object);

        // Act
        var result = await repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(description, result.Description);
        Assert.Equal(details, result.Details);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid(); // ID not in DB

        var repository = new SubjectRepository(context, _mapper.Object);

        // Act
        var result = await repository.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }
    
}
