namespace Application.DTO;

public record CreateSubjectDTO
{
    public required string Description { get; set; }
    public required string Details { get; set; }

}
