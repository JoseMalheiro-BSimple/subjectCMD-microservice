using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Models;
public class Subject : ISubject
{
    public Guid Id { get; }
    public Description Description { get; }
    public Details Details {get; }

    public Subject(Guid id, Description description, Details details)
    {
        Id = id;
        Description = description;
        Details = details;
    }
}
