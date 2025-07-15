using Application.IService;
using Domain.Messages;
using InterfaceAdapters.Consumers;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.ConsumerTests;

public class SubjectCreatedConsumerTests
{
    [Fact]
    public async Task Consume_CallsCreateWithNoValidation_WithCorrectValues()
    {
        // Arrange
        var mockService = new Mock<ISubjectService>();
        var consumer = new SubjectCreatedConsumer(mockService.Object);

        var message = new SubjectCreatedMessage(
            Guid.NewGuid(),
            "Test description",
            "Test details");

        var mockContext = new Mock<ConsumeContext<SubjectCreatedMessage>>();
        mockContext.Setup(c => c.Message).Returns(message);

        // Act
        await consumer.Consume(mockContext.Object);

        // Assert
        mockService.Verify(service =>
            service.CreateWithNoValidation(
                message.Id,
                message.Description,
                message.Details),
            Times.Once);
    }
}
