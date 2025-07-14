using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.Tests;

public class RepositoryTestBase
{
    protected readonly Mock<IMapper> _mapper;
    protected readonly SubjectCMDContext context;

    protected RepositoryTestBase()
    {
        _mapper = new Mock<IMapper>();

        // Configure in-memory EF Core context
        var options = new DbContextOptionsBuilder<SubjectCMDContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        context = new SubjectCMDContext(options);
    }
}
