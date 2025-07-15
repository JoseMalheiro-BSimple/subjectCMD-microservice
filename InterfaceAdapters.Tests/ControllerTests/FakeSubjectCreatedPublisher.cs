using Application.IPublisher;
using Domain.ValueObjects;

namespace InterfaceAdapters.Controllers;

public class FakeSubjectCreatedPublisher : IMassTransitPublisher
{
    public Task PublishCreatedSubjectCreatedMessage(Guid id, Description description, Details details)
    {
        // Simulate success without doing anything
        return Task.CompletedTask;
    }
}
