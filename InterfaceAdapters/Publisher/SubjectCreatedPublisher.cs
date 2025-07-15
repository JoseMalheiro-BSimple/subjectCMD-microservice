using Application.IPublisher;
using Domain.Messages;
using Domain.ValueObjects;
using MassTransit;

namespace InterfaceAdapters.Publisher;

public class SubjectCreatedPublisher : IMassTransitPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public SubjectCreatedPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishCreatedSubjectCreatedMessage(Guid id, Description description, Details details)
    {
        await _publishEndpoint.Publish(new SubjectCreatedMessage(id, description.Value, details.Value));
    }
}
