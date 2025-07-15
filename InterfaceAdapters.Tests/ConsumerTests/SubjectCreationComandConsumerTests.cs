using Application.DTO;
using Application.IService;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class SubjectCreationComandConsumerTests
{
    [Fact]
    public async Task Consume_CallsCreate_WithCorrectValues()
    {
        // Arrange
        var mockService = new Mock<ISubjectService>();
        var consumer = new SubjectCreationComandConsumer(mockService.Object);

        var message = new SubjectCreationCommand(
            "Test description",
            "Test details");

        var mockContext = new Mock<ConsumeContext<SubjectCreationCommand>>();
        mockContext.Setup(c => c.Message).Returns(message);

        var createDTO = new CreateSubjectDTO { Description = message.Description, Details = message.Details };

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(service =>
            service.Create(createDTO),
            Times.Once);
    }
}
