using Application.DTO;
using Application.IPublisher;
using Application.IService;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services;

public class SubjectService : ISubjectService
{
    private ISubjectFactory _subjectFactory;
    private ISubjectRepository _subjectRepository;
    private IMassTransitPublisher _publisher;

    public SubjectService(ISubjectFactory subjectFactory, ISubjectRepository subjectRepository, IMassTransitPublisher publisher)
    {
        _subjectFactory = subjectFactory;
        _subjectRepository = subjectRepository;
        _publisher = publisher;
    }

    public async Task<Result<CreatedSubjectDTO>> Create(CreateSubjectDTO createSubjectDTO)
    {
        ISubject subject = null!;
        try
        {
            subject = await _subjectFactory.Create(createSubjectDTO.Description, createSubjectDTO.Details);
            subject = await _subjectRepository.AddAsync(subject);

            if (subject == null)
                throw new Exception("DB error!");

            await _publisher.PublishCreatedSubjectCreatedMessage(subject.Id, subject.Description, subject.Details);

            var result = new CreatedSubjectDTO
            {
                Id = subject.Id,
                Description = subject.Description.Value,
                Details = subject.Details.Value,
            };

            return Result<CreatedSubjectDTO>.Success(result);

        } catch(Exception ex)
        {
            if (subject != null)
            {
                try
                {
                    await _subjectRepository.DeleteAsync(subject.Id);
                }
                catch (Exception cleanupEx)
                {
                    // Optional: log cleanupEx
                    return Result<CreatedSubjectDTO>.Failure(
                        Error.InternalServerError($"Publish failed: {ex.Message}; Cleanup also failed: {cleanupEx.Message}")
                    );
                }
            }

            return Result<CreatedSubjectDTO>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task CreateWithNoValidation(Guid id, string description, string details)
    {
        ISubject? subject = await _subjectRepository.GetByIdAsync(id);

        if(subject == null)
        {
            ISubject toAdd = _subjectFactory.Create(id, description, details);

            await _subjectRepository.AddAsync(toAdd);
        }
    }
}
