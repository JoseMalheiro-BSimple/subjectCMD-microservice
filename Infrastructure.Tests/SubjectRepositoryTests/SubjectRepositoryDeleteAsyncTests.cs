using Domain.ValueObjects;
using Infrastructure.DataModel;
using Infrastructure.Repositories;

namespace Infrastructure.Tests.SubjectRepositoryTests;

public class SubjectRepositoryDeleteAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task DeleteAsync_ShouldRemoveSubject_WhenSubjectExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var subject = new SubjectDataModel
        {
            Id = id,
            Description = new Description("Delete Me"),
            Details = new Details("To be removed")
        };

        context.Subjects.Add(subject);
        await context.SaveChangesAsync();

        var repository = new SubjectRepository(context, _mapper.Object);

        // Act
        await repository.DeleteAsync(id);

        // Assert
        var result = await context.Subjects.FindAsync(id);
        Assert.Null(result); 
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothing_WhenSubjectDoesNotExist()
    {
        // Arrange
        var repository = new SubjectRepository(context, _mapper.Object);
        var nonExistentId = Guid.NewGuid();

        // Act
        var exception = await Record.ExceptionAsync(() => repository.DeleteAsync(nonExistentId));

        // Assert
        Assert.Null(exception); 
    }
}
