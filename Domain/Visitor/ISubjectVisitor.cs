using Domain.ValueObjects;

namespace Domain.Visitor;

public interface ISubjectVisitor
{
    Guid Id { get; }
    Description Description { get; }
    Details Details { get; }
}
