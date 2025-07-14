using Domain.ValueObjects;

namespace Domain.Messages;
public record SubjectCreatedMessage(Guid Id, Description Description, Details Details);