namespace MsClean.Application;
using AutoMapper;
using MsClean.Domain;

public class ProfileMapping : Profile
{
    public ProfileMapping()
    {
        CreateMap<Person, PersonViewModel>()
               .ForMember(t => t.Id, o => o.MapFrom(t => t.Id))
               .ForMember(t => t.Name, o => o.MapFrom(t => t.Name))
               .ForMember(t => t.LastName, o => o.MapFrom(t => t.LastName))
               .ForMember(t => t.LastName, o => o.MapFrom(t => t.LastName))
               .ForMember(t => t.IsActive, o => o.MapFrom(t => t.IsActive))
               .ReverseMap();
    }
}
