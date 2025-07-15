using Application.DTO;

namespace InterfaceAdapters.Tests.ControllerTests;

public class SubjectControllerCreateTests : IntegrationTestBase, IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;

    public SubjectControllerCreateTests(IntegrationTestsWebApplicationFactory<Program> factory) : base(factory.CreateClient())
    {
        _factory = factory;
    }

    [Fact]
    public async Task Create_ReturnsCreatedSubjectDTO()
    {
        // Arrange
        var description = "Test description";
        var details = "Test details";

        var createDTO = new CreateSubjectDTO { Description = description, Details = details };

        // Act
        var response = await PostAndDeserializeAsync<CreatedSubjectDTO>("api/subjects", createDTO);

        // Assert

        Assert.NotNull(response);
        Assert.Equal(description, response.Description);
        Assert.Equal(details, response.Details);
    }
}
