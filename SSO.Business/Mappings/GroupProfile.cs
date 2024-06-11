using AutoMapper;
using SSO.Business.Groups;
using SSO.Business.Groups.Commands;
using SSO.Domain.Models;

namespace SSO.Business.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<CreateGroupCommand, Group>()
                .ForMember(x => x.GroupId, from => from.MapFrom(x => Guid.NewGuid()))
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();

            CreateMap<UpdateGroupCommand, Group>()
                .IgnoreAllSourcePropertiesWithAnInaccessibleSetter();
        }
    }
}
