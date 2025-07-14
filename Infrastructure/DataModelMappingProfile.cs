using AutoMapper;
using Domain.Interfaces;
using Infrastructure.DataModel;
using Infrastructure.Resolvers;

namespace Infrastructure;

public class DataModelMappingProfile : Profile
{
    public DataModelMappingProfile()
    {
        CreateMap<ISubject, SubjectDataModel>();
        CreateMap<SubjectDataModel, ISubject>()
            .ConvertUsing<SubjectDataModelConverter>();
    }
}
