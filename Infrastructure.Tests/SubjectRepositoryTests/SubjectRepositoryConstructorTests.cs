using Infrastructure.Repositories;

namespace Infrastructure.Tests.SubjectRepositoryTests;

public class SubjectRepositoryConstructorTests : RepositoryTestBase
{
    [Fact]
    public void Constructor_ShouldInstantiateSubjectRepository()
    {
        // Act
        var repository = new SubjectRepository(context, _mapper.Object);

        // Assert
        Assert.NotNull(repository);
    }
}

