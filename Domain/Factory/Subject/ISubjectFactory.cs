using Domain.Interfaces;
using Domain.Visitor;

namespace Domain.Factory;

public interface ISubjectFactory
{
    Task<ISubject> Create(string description, string details);
    ISubject Create(Guid id, string description, string details);
    ISubject Create(ISubjectVisitor visitor);
}
