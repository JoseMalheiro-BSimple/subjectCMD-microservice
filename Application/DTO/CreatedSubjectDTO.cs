namespace Application.DTO;

public record CreatedSubjectDTO
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
    public required string Details { get; set; }
}
