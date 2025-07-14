using Domain.ValueObjects;

namespace Application.IPublisher;

public interface IMassTransitPublisher
{
    Task PublishCreatedSubjectCreatedMessage(Guid id, Description description, Details details);
}
