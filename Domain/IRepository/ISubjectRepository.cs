using Domain.Interfaces;

namespace Domain.IRepository;

public interface ISubjectRepository
{
    // Generic methods
    ISubject? GetById(Guid id);
    Task<ISubject?> GetByIdAsync(Guid id);
    ISubject Add(ISubject newSubject);
    Task<ISubject> AddAsync(ISubject newSubject);

    // Subject specifics
    Task<ISubject?> GetSubjectByDescription(string description);
}
