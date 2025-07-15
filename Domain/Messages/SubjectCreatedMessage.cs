using Domain.ValueObjects;

namespace Domain.Messages;
public record SubjectCreatedMessage(Guid Id, string Description, string Details);