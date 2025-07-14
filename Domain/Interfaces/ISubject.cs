using Domain.ValueObjects;

namespace Domain.Interfaces;

public interface ISubject
{
    Guid Id { get; }
    Description Description { get; }
    Details Details { get; }
}
