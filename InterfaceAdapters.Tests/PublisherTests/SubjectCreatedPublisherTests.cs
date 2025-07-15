using Domain.Messages;
using Domain.ValueObjects;
using InterfaceAdapters.Publisher;
using MassTransit;
using Moq;

namespace InterfaceAdapters.Tests.PublisherTests;

public class SubjectCreatedPublisherTests
{
    [Fact]
    public async Task PublishCreatedSubjectCreatedMessage_PublishesCorrectMessage()
    {
        // Arrange
        var mockPublishEndpoint = new Mock<IPublishEndpoint>();
        var publisher = new SubjectCreatedPublisher(mockPublishEndpoint.Object);

        var id = Guid.NewGuid();
        var description = new Description("Test description");
        var details = new Details("Test details");

        SubjectCreatedMessage? publishedMessage = null;

        mockPublishEndpoint
            .Setup(x => x.Publish(
                It.IsAny<SubjectCreatedMessage>(),
                It.IsAny<CancellationToken>()))
            .Callback<object, CancellationToken>((msg, _) =>
            {
                publishedMessage = msg as SubjectCreatedMessage;
            })
            .Returns(Task.CompletedTask);

        // Act
        await publisher.PublishCreatedSubjectCreatedMessage(id, description, details);

        // Assert
        Assert.NotNull(publishedMessage);
        Assert.Equal(id, publishedMessage!.Id);
        Assert.Equal(description.Value, publishedMessage.Description);
        Assert.Equal(details.Value, publishedMessage.Details);

        mockPublishEndpoint.Verify(x =>
            x.Publish(It.IsAny<SubjectCreatedMessage>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
