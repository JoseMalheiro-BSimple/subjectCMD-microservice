using Application.DTO;

namespace Application.IService;

public interface ISubjectService
{
    Task<Result<CreatedSubjectDTO>> Create(CreateSubjectDTO createSubjectDTO);

    Task CreateWithNoValidation(Guid id, string description, string details);
}
