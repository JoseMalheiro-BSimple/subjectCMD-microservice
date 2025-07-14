using Application.DTO;

namespace Application.IService;

public interface ISubjectService
{
    Task<Result<CreatedSubjectDTO>> Create(CreateSubjectDTO createSubjectDTO);
}
