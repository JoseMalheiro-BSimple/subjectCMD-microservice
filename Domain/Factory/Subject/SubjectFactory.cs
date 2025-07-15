using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Domain.ValueObjects;
using Domain.Visitor;

namespace Domain.Factory;

public class SubjectFactory : ISubjectFactory
{
    private ISubjectRepository _subjectRepository;

    public SubjectFactory(ISubjectRepository subjectRepository)
    {
        _subjectRepository = subjectRepository;
    }

    public async Task<ISubject> Create(string description, string details)
    {
        // Unicity test
        ISubject? subject = await _subjectRepository.GetSubjectByDescription(description);

        if (subject != null)
            throw new ArgumentException("The subject's description already exists!");
        
        Description newDescr = new Description(description);
        Details Det = new Details(details);

        Guid id = Guid.NewGuid();

        return new Subject(id, newDescr, Det);
    }

    public ISubject Create(Guid id, string description, string details)
    {
        Description newDescr = new Description(description);
        Details Det = new Details(details);

        return new Subject(id, newDescr, Det);
    }

    public ISubject Create(ISubjectVisitor visitor)
    {
        return new Subject(visitor.Id, visitor.Description, visitor.Details);
    }
}
