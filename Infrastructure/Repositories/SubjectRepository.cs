using AutoMapper;
using Domain.Interfaces;
using Domain.IRepository;
using Infrastructure.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class SubjectRepository : ISubjectRepository
{
    protected readonly DbContext _context;
    protected readonly IMapper _mapper;

    public SubjectRepository(SubjectCMDContext dbContext, IMapper mapper)
    {
        _context = dbContext;
        _mapper = mapper;
    }

    public ISubject Add(ISubject newSubject)
    {
        var dataModel = _mapper.Map<ISubject, SubjectDataModel>(newSubject);
        var dm = _context.Set<SubjectDataModel>().Add(dataModel);

        _context.SaveChanges();

        return _mapper.Map<SubjectDataModel, ISubject>(dm.Entity);
    }

    public async Task<ISubject> AddAsync(ISubject newSubject)
    {
        var dataModel = _mapper.Map<ISubject, SubjectDataModel>(newSubject);
        _context.Set<SubjectDataModel>().Add(dataModel);
        
        await _context.SaveChangesAsync();

        return _mapper.Map<SubjectDataModel, ISubject>(dataModel);
    }

    public ISubject? GetById(Guid id)
    {
        var subjectDM = _context.Set<SubjectDataModel>().FirstOrDefault(c => c.Id == id);

        if (subjectDM == null)
            return null;

        var subject = _mapper.Map<SubjectDataModel, ISubject>(subjectDM);
        return subject;
    }

    public async Task<ISubject?> GetByIdAsync(Guid id)
    {
        var subjectDM = await _context.Set<SubjectDataModel>().FirstOrDefaultAsync(c => c.Id == id);

        if (subjectDM == null)
            return null;

        var subject = _mapper.Map<SubjectDataModel, ISubject>(subjectDM);
        return subject;
    }

    public async Task<ISubject?> GetSubjectByDescription(string description)
    {
        var normalizedInput = description.ToLowerInvariant();

        var subjectDM = await _context.Set<SubjectDataModel>()
            .FirstOrDefaultAsync(s => EF.Functions.ILike(s.Description.Value, description));

        if (subjectDM == null)
            return null;

        return _mapper.Map<SubjectDataModel, ISubject>(subjectDM);
    }
}
