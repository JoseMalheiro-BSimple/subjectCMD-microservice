using AutoMapper;
using Domain.Factory;
using Domain.Interfaces;
using Infrastructure.DataModel;

namespace Infrastructure.Resolvers;

public class SubjectDataModelConverter : ITypeConverter<SubjectDataModel, ISubject>
{
    private readonly ISubjectFactory _subjectFactory;

    public SubjectDataModelConverter(ISubjectFactory subjectFactory)
    {
        _subjectFactory = subjectFactory;
    }

    public ISubject Convert(SubjectDataModel source, ISubject destination, ResolutionContext context)
    {
        return _subjectFactory.Create(source);
    }
}
