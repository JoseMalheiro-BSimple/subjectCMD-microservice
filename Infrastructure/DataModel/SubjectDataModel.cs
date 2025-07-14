using Domain.Interfaces;
using Domain.ValueObjects;
using Domain.Visitor;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.DataModel;

[Table("Subject")]
public class SubjectDataModel : ISubjectVisitor
{
    public Guid Id {  get; set; }
    public required Description Description {  get; set; }
    public required Details Details {  get; set; }

    public SubjectDataModel() { }

    public SubjectDataModel(ISubject subject)
    {
        Id = subject.Id;
        Description = subject.Description;
        Details = subject.Details;
    }
}
